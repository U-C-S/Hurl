using Hurl.Library;
using Hurl.Selector.Helpers;
using Hurl.Selector.Models;
using Hurl.Selector.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using System;
using System.Diagnostics;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using WinUIEx;

namespace Hurl.Selector.Pages;

public sealed partial class SelectorWindow : Window
{
    private const uint TrayIconId = 3721;

    public SelectorPageViewModel ViewModel { get; }

    private WindowManager? windowManager;
    private readonly TrayIcon trayIcon;
    private readonly MenuFlyout trayMenuFlyout;
    private bool trayIconDisposed;

    public SelectorWindow()
    {
        ViewModel = App.Services.GetRequiredService<SelectorPageViewModel>();
        ExtendsContentIntoTitleBar = true;
        this.AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;

        windowManager = WindowManager.Get(this);
        windowManager.IsMaximizable = false;
        windowManager.IsMinimizable = false;
        windowManager.MinWidth = 500;
        windowManager.MinHeight = 250;

        //this.AppWindow.IsShownInSwitchers = false;
        AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(600, 320));
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

    private void MinimizeWindow()
    {
        this.Minimize();
        this.Hide();
    }

    public void ShowWindow()
    {
        this.Show();
        this.Restore();
        Activate();
        this.SetForegroundWindow();
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

    private void NotifyIcon_LeftClick(object? sender, TrayIconEventArgs e)
    {
        e.Handled = true;
        ShowWindow();
    }

    private void NotifyIcon_ContextMenu(object? sender, TrayIconEventArgs e)
    {
        e.Handled = true;
        e.Flyout = trayMenuFlyout;
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
#if DEBUG
        // No minimize on debug when not in focus
#else
        //var appSettings = Settings.AppSettings;
        //if (!forcePreventWindowDeactivationEvent && appSettings.MinimizeOnFocusLoss) MinimizeWindow();
#endif
    }

    private void PositionWindowUnderTheMouse()
    {
        //var appSettings = Settings.appSettings;

        //try
        //{
        //    if (appSettings?.LaunchUnderMouse == true)
        //    {
        //        var transform = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformFromDevice;
        //        if (transform is Matrix t)
        //        {
        //            var mouse = t.Transform(CursorPosition.LimitCursorWithin((int)Width, (int)Height));
        //            Left = mouse.X;
        //            Top = mouse.Y;

        //            Debug.WriteLine($"{Left}?{Top} with screen resolution: {SystemParameters.FullPrimaryScreenWidth}?{SystemParameters.FullPrimaryScreenHeight}");
        //        }
        //    }
        //}
        //catch (Exception) { }
    }

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

    private void BrowserButton_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Browser browser)
        {
            button.ContextFlyout = CreateAlternateLaunchFlyout(browser);
        }
    }

    private void AdditionalBtn_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Browser browser)
        {
            var flyout = CreateAlternateLaunchFlyout(browser);
            button.Flyout = flyout;
            FlyoutBase.SetAttachedFlyout(button, flyout);
        }
    }

    private void AdditionalBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            FlyoutBase.ShowAttachedFlyout(button);
        }
    }

    private MenuFlyout? CreateAlternateLaunchFlyout(Browser browser)
    {
        if (browser.AlternateLaunches is not { Count: > 0 } alternateLaunches)
        {
            return null;
        }

        MenuFlyout flyout = new();

        foreach (var alternateLaunch in alternateLaunches)
        {
            MenuFlyoutItem item = new()
            {
                Text = alternateLaunch.ItemName,
                Tag = new AlternateLaunchContext(browser, alternateLaunch)
            };
            item.Click += AlternateLaunch_Click;
            flyout.Items.Add(item);
        }

        return flyout;
    }

    private void AlternateLaunch_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuFlyoutItem { Tag: AlternateLaunchContext context })
        {
            return;
        }

        try
        {
            UriLauncher.Alternative(ViewModel.Url, context.Browser, context.AlternateLaunch);
            MinimizeWindow();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private sealed record AlternateLaunchContext(Browser Browser, AlternateLaunch AlternateLaunch);

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

        args.Handled = true;
        UrlTextBox.Focus(FocusState.Keyboard);
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

    private void CopyCurrentUrlToClipboard()
    {
        DataPackage package = new();
        package.SetText(ViewModel.Url ?? string.Empty);
        Clipboard.SetContent(package);
        Clipboard.Flush();
    }

    //private async System.Threading.Tasks.Task UrlEditAsync()
    //{
    //    TextBox editor = new()
    //    {
    //        AcceptsReturn = false,
    //        PlaceholderText = "Enter the URL you want to open",
    //        Text = ViewModel.Url
    //    };
    //    editor.Loaded += (_, _) =>
    //    {
    //        editor.SelectAll();
    //        editor.Focus(FocusState.Programmatic);
    //    };

    //    ContentDialog dialog = new()
    //    {
    //        XamlRoot = (Content as FrameworkElement)?.XamlRoot,
    //        Title = "Edit URL to open",
    //        Content = editor,
    //        CloseButtonText = "Cancel",
    //        PrimaryButtonText = "OK",
    //        DefaultButton = ContentDialogButton.Primary
    //    };

    //    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
    //    {
    //        ViewModel.Url = editor.Text?.Trim() ?? string.Empty;
    //        UrlTextBox.Focus(FocusState.Programmatic);
    //        UrlTextBox.SelectAll();
    //    }
    //}

    private bool IsTextBoxKeyAccelerator()
    {
        var xamlRoot = (Content as FrameworkElement)?.XamlRoot;
        return xamlRoot is not null && FocusManager.GetFocusedElement(xamlRoot) is TextBox;
    }

    private TrayIcon CreateTrayIcon()
    {
        string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "internet.ico");
        TrayIcon icon = new(TrayIconId, iconPath, "Hurl is running in background for faster access")
        {
            IsVisible = true
        };

        icon.Selected += NotifyIcon_LeftClick;
        icon.LeftDoubleClick += NotifyIcon_LeftClick;
        icon.ContextMenu += NotifyIcon_ContextMenu;

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
        CleanupTrayIcon();
        Application.Current.Exit();
    }

    private void SelectorWindow_Closed(object sender, WindowEventArgs args)
    {
        CleanupTrayIcon();
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
        trayIcon.Selected -= NotifyIcon_LeftClick;
        trayIcon.LeftDoubleClick -= NotifyIcon_LeftClick;
        trayIcon.ContextMenu -= NotifyIcon_ContextMenu;

        trayIcon.Dispose();
    }
}
