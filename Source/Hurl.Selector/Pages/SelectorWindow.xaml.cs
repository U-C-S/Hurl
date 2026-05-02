using Hurl.Library;
using Hurl.Selector.Controls;
using Hurl.Selector.Helpers;
using Hurl.Selector.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Diagnostics;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using WinRT;
using WinUIEx;

namespace Hurl.Selector.Pages;

[GeneratedBindableCustomProperty]
public sealed partial class SelectorWindow : Window
{
    public SelectorPageViewModel ViewModel { get; }

    private WindowManager? windowManager;

    private const uint TrayIconId = 3721;
    private readonly TrayIcon trayIcon;
    private readonly MenuFlyout trayMenuFlyout;
    private bool allowWindowClose;
    private bool trayIconDisposed;
    private bool isHiddenToTray;

    #region Window Lifecycle
    public SelectorWindow()
    {
        ViewModel = App.Services.GetRequiredService<SelectorPageViewModel>();
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
        trayMenuFlyout = CreateTrayMenuFlyout();
        trayIcon = CreateTrayIcon();

        InitializeComponent();
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
        if (!allowWindowClose)
        {
            args.Handled = true;
            MinimizeWindow();
            return;
        }

        CleanupTrayIcon();
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
    private void MinimizeWindow()
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

    private void ReloadApp()
    {
        string appPath = Environment.ProcessPath ?? Path.Combine(AppContext.BaseDirectory, "Hurl Selector.exe");
        Process.Start(new ProcessStartInfo(appPath)
        {
            UseShellExecute = true
        });
        ExitApp();
    }

    private void ExitApp()
    {
        allowWindowClose = true;
        Application.Current.Exit();
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

    private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start(Constants.SETTINGS_APP, "--page settings");

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
        MinimizeWindow();
        Process.Start(Constants.SETTINGS_APP, "--page rulesets");
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

        MinimizeWindow();
        Process.Start(Constants.SETTINGS_APP, "--page rulesets");
        args.Handled = true;
    }

    private bool IsTextBoxKeyAccelerator()
    {
        var xamlRoot = (Content as FrameworkElement)?.XamlRoot;
        return xamlRoot is not null && FocusManager.GetFocusedElement(xamlRoot) is TextBox;
    }
    #endregion

    #region TrayIcon Lifecycle methods
    private TrayIcon CreateTrayIcon()
    {
        string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "internet.ico");
        TrayIcon icon = new(TrayIconId, iconPath, "Hurl is running in background for faster access")
        {
            IsVisible = true
        };

        icon.Selected += TrayIcon_LeftClick;
        icon.LeftDoubleClick += TrayIcon_LeftClick;
        icon.ContextMenu += TrayIcon_ContextMenu;

        return icon;
    }

    private MenuFlyout CreateTrayMenuFlyout()
    {
        MenuFlyout flyout = new();
        flyout.Items.Add(CreateTrayMenuItem("Settings", "settings", "\uE713"));
        flyout.Items.Add(CreateTrayMenuItem("Reload", "reload", "\uE777"));
        flyout.Items.Add(CreateTrayMenuItem("Exit", "exit", "\uE8BB"));
        return flyout;
    }

    private MenuFlyoutItem CreateTrayMenuItem(string text, string tag, string glyph)
    {
        MenuFlyoutItem item = new()
        {
            Text = text,
            Tag = tag,
            Icon = new FontIcon { Glyph = glyph }
        };
        item.Click += TrayMenuItem_OnClick;
        return item;
    }

    private void CleanupTrayIcon()
    {
        if (trayIconDisposed)
        {
            return;
        }

        trayIconDisposed = true;
        trayIcon.CloseFlyout();
        trayIcon.IsVisible = false;
        trayIcon.Selected -= TrayIcon_LeftClick;
        trayIcon.LeftDoubleClick -= TrayIcon_LeftClick;
        trayIcon.ContextMenu -= TrayIcon_ContextMenu;

        trayIcon.Dispose();
    }
    #endregion

    #region TrayIcon Event Handlers
    private void TrayIcon_LeftClick(object? sender, TrayIconEventArgs e)
    {
        e.Handled = true;
        ShowWindow();
    }

    private void TrayIcon_ContextMenu(object? sender, TrayIconEventArgs e)
    {
        e.Handled = true;
        e.Flyout = trayMenuFlyout;
    }

    private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        string? tag = (sender as MenuFlyoutItem)?.Tag as string;

        try
        {
            switch (tag)
            {
                case "settings":
                    Process.Start(Constants.SETTINGS_APP, "--page settings");
                    break;
                case "reload":
                    ReloadApp();
                    break;
                case "exit":
                    ExitApp();
                    break;
                default:
                    break;
            }
        }
        catch (Exception err)
        {
            Debug.WriteLine(err);
        }
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
