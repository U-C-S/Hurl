using Hurl.BrowserSelector.Helpers;
using Hurl.Library;
using Hurl.Selector.Models;
using Hurl.Selector.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Diagnostics;
using WinUIEx;

namespace Hurl.Selector.Pages;

public sealed partial class SelectorWindow : Window
{
    public SelectorPageViewModel ViewModel { get; }

    WindowManager? windowManager;

    public SelectorWindow()
    {
        ViewModel = App.Services.GetRequiredService<SelectorPageViewModel>();
        this.ExtendsContentIntoTitleBar = true;
        //this.AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Collapsed;

        windowManager = WindowManager.Get(this);
        windowManager.IsMaximizable = false;
        windowManager.IsMinimizable = false;
        windowManager.MinWidth = 500;
        windowManager.MinHeight = 250;

        //this.AppWindow.IsShownInSwitchers = false;
        this.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(600, 320));

        this.InitializeComponent();
    }

    private void LinkCopyBtnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            //Clipboard.SetText(OpenedUri.Value);
        }
        catch (Exception err)
        {
            System.Windows.MessageBox.Show(err.Message);
        }
    }

    private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start(Constants.SETTINGS_APP, "--page settings");

    private void CloseBtnClick(object sender, RoutedEventArgs e) => MinimizeWindow();

    private void MinimizeWindow()
    {
        //WindowState = WindowState.Minimized;
        //Hide();
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

        //            Debug.WriteLine($"{Left}�{Top} with screen resolution: {SystemParameters.FullPrimaryScreenWidth}�{SystemParameters.FullPrimaryScreenHeight}");
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
        var x = sender.GetType();
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
}
