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
using Windows.ApplicationModel.DataTransfer;
using WinUIEx;

namespace Hurl.Selector.Pages;

public sealed partial class SelectorWindow : Window
{
    public SelectorPageViewModel ViewModel { get; }

    private WindowManager? windowManager;

    public SelectorWindow()
    {
        ViewModel = App.Services.GetRequiredService<SelectorPageViewModel>();
        ExtendsContentIntoTitleBar = true;
        //this.AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Collapsed;

        windowManager = WindowManager.Get(this);
        windowManager.IsMaximizable = false;
        windowManager.IsMinimizable = false;
        windowManager.MinWidth = 500;
        windowManager.MinHeight = 250;

        //this.AppWindow.IsShownInSwitchers = false;
        AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(600, 320));

        InitializeComponent();
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
    }

    public void ShowWindow()
    {
        //Show();
        //PositionWindowUnderTheMouse();
        //WindowState = WindowState.Normal;
    }

    private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        //string? tag = ((MenuItem)sender).Tag as string;
        //try
        //{
        //    switch (tag)
        //    {
        //        case "open":
        //            ShowWindow();
        //            break;
        //        case "settings":
        //            Process.Start(Constants.SETTINGS_APP, "--page settings");
        //            break;
        //        case "exit":
        //            Application.Current.Exit();
        //            break;
        //        case "reload":
        //            var AppPath = Path.Combine(AppContext.BaseDirectory, "Hurl.exe");
        //            Process.Start(AppPath);
        //            Application.Current.Exit();
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //catch (Exception err)
        //{
        //    System.Windows.MessageBox.Show(err.Message);
        //}
    }

    private void NotifyIcon_LeftClick(object sender, RoutedEventArgs e) => ShowWindow();

    private void Window_Deactivated(object sender, EventArgs e)
    {
#if DEBUG
        // No minimize on debug when not in focus
#else
        var appSettings = Settings.AppSettings;
        if (!forcePreventWindowDeactivationEvent && appSettings.MinimizeOnFocusLoss) MinimizeWindow();
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
}
