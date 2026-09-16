using Hurl.App.Controls;
using Hurl.App.Helpers;
using Hurl.App.Services.Interfaces;
using Hurl.App.ViewModels;
using Hurl.Library;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using WinRT;
using WinUIEx;

namespace Hurl.App.Windows;

[GeneratedBindableCustomProperty]
public sealed partial class SelectorWindow : Window
{
    public SelectorPageViewModel ViewModel { get; }
    private readonly IQuickViewService quickViewService;
    private readonly ISettingsService settingsService;

    private WindowManager? windowManager;

    private bool isHiddenToTray;

    #region Window Lifecycle
    public SelectorWindow()
    {
        IServiceProvider services = App.Services ?? throw new InvalidOperationException("Application services are not configured.");
        ViewModel = services.GetRequiredService<SelectorPageViewModel>();
        quickViewService = services.GetRequiredService<IQuickViewService>();
        settingsService = services.GetRequiredService<ISettingsService>();
        ViewModel.BrowserLaunched += ViewModel_BrowserLaunched;
        ExtendsContentIntoTitleBar = true;
        this.AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;

        windowManager = WindowManager.Get(this);
        windowManager.IsMaximizable = false;
        windowManager.IsMinimizable = false;
        windowManager.IsAlwaysOnTop = true;
        windowManager.MinWidth = 500;
        windowManager.MinHeight = 260;

        //this.AppWindow.IsShownInSwitchers = false;
        ApplyConfiguredWindowSize();
        Activated += Window_Activated;
        Closed += SelectorWindow_Closed;

        InitializeComponent();
        ApplyConfiguredBackground();
        QuickViewButton.IsEnabled = quickViewService.IsQuickViewEnabled;
        settingsService.SettingsChanged += SettingsChanged;
    }

    public void Init(CliArgs args)
    {
        ViewModel.Url = args.Url;

        if (args.IsRunAsMin)
        {
            Activate();
            MinimizeWindow();
            return;
        }

        ShowWindow();
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        if (isHiddenToTray)
        {
            return;
        }

#if DEBUG
        // No minimize on debug when not in focus
#else
        if (ViewModel.AppSettings.MinimizeOnFocusLoss)
        {
            MinimizeWindow();
        }
#endif
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState == WindowActivationState.Deactivated)
        {
            Window_Deactivated(sender, EventArgs.Empty);
        }
    }

    private void SelectorWindow_Closed(object sender, WindowEventArgs args)
    {
        if (!App.IsExiting)
        {
            args.Handled = true;
            MinimizeWindow();
            return;
        }

        settingsService.SettingsChanged -= SettingsChanged;
        ViewModel.BrowserLaunched -= ViewModel_BrowserLaunched;
        Activated -= Window_Activated;
    }

    private void PositionWindowUnderTheMouse()
    {
        try
        {
            if (!ViewModel.AppSettings.LaunchUnderMouse)
            {
                return;
            }

            var (width, height) = GetConfiguredWindowSize();
            var scale = (Content as FrameworkElement)?.XamlRoot?.RasterizationScale ?? 1.0;
            var position = CursorPosition.LimitCursorWithin(
                (int)Math.Round(width * scale),
                (int)Math.Round(height * scale));

            this.MoveAndResize(position.X / scale, position.Y / scale, width, height);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
    #endregion

    #region Window Lifecycle Helper methods
    internal void MinimizeWindow()
    {
        isHiddenToTray = true;
        this.Minimize();
        this.Hide();
    }

    public void ShowWindow()
    {
        isHiddenToTray = false;
        this.Show();
        this.Restore();
        PositionWindowUnderTheMouse();
        Activate();
        this.SetForegroundWindow();
    }

    private void ApplyConfiguredBackground()
    {
        string? backgroundType = ViewModel.AppSettings.BackgroundType?.ToLowerInvariant();
        if (backgroundType == "acrylic")
        {
            if (SystemBackdrop is not DesktopAcrylicBackdrop)
            {
                SystemBackdrop = new DesktopAcrylicBackdrop();
            }
        }
        else if (SystemBackdrop is not MicaBackdrop)
        {
            SystemBackdrop = new MicaBackdrop();
        }
    }

    private void ApplyConfiguredWindowSize()
    {
        var (width, height) = GetConfiguredWindowSize();
        this.SetWindowSize(width, height);
    }

    private (double Width, double Height) GetConfiguredWindowSize()
    {
        var windowSize = ViewModel.AppSettings.WindowSize;
        double width = windowSize is { Length: >= 1 } ? windowSize[0] : 500;
        double height = windowSize is { Length: >= 2 } ? windowSize[1] : 260;

        width = Math.Max(width, windowManager?.MinWidth ?? 500);
        height = Math.Max(height, windowManager?.MinHeight ?? 260);
        return (width, height);
    }

    #endregion

    #region Selector UI Event Handlers
    private void ViewModel_BrowserLaunched(object? sender, EventArgs e) => MinimizeWindow();

    private void LinkCopyBtnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            CopyCurrentUrlToClipboard();
        }
        catch (Exception err)
        {
            Debug.WriteLine(err);
        }
    }

    private void SettingsChanged(object? sender, EventArgs e)
    {
        ViewModel.RefreshSettings();
        ApplyConfiguredBackground();
        QuickViewButton.IsEnabled = quickViewService.IsQuickViewEnabled;
        ApplyConfiguredWindowSize();
    }

    private void SettingsBtnClick(object sender, RoutedEventArgs e) => App.ShowSettings("settings");

    private void CloseBtnClick(object sender, RoutedEventArgs e) => MinimizeWindow();

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        //forcePreventWindowDeactivationEvent = true;
        //new TimeSelectWindow(Settings.browsers).ShowDialog();
        //forcePreventWindowDeactivationEvent = false;
    }

    //private void Window_SizeChanged(object sender, SizeChangedEventArgs e) => Settings.AdjustWindowSize(e);

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        App.ShowSettings("rulesets");
    }

    private void BrowserBarButton_AlternateLaunchRequested(object? sender, AlternateLaunchRequestedEventArgs e)
    {
        try
        {
            UriLauncher.Alternative(ViewModel.Url, e.Browser, e.AlternateLaunch);
            MinimizeWindow();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private void QuickViewBtnClick(object sender, RoutedEventArgs e)
    {
        if (quickViewService.TryOpen(ViewModel.Url))
        {
            MinimizeWindow();
        }
    }
    #endregion

    #region Keyboard Accelerators
    private void EscapeAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        MinimizeWindow();
        args.Handled = true;
    }

    private void CopyAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        if (IsTextBoxKeyAccelerator())
        {
            return;
        }

        CopyCurrentUrlToClipboard();
        args.Handled = true;
    }

    private async void EditUrlAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        if (IsTextBoxKeyAccelerator())
        {
            return;
        }

        UrlTextBox.Focus(FocusState.Keyboard);
        args.Handled = true;
    }

    private void RulesAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        if (IsTextBoxKeyAccelerator())
        {
            return;
        }

        App.ShowSettings("rulesets");
        args.Handled = true;
    }

    private bool IsTextBoxKeyAccelerator()
    {
        var xamlRoot = (Content as FrameworkElement)?.XamlRoot;
        return xamlRoot is not null && FocusManager.GetFocusedElement(xamlRoot) is TextBox;
    }
    #endregion

    #region Helper methods
    private void CopyCurrentUrlToClipboard()
    {
        DataPackage package = new();
        package.SetText(ViewModel.Url ?? string.Empty);
        Clipboard.SetContent(package);
        Clipboard.Flush();
    }

    #endregion
}
