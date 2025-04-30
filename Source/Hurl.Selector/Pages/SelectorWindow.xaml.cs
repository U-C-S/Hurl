using Hurl.Library;
using Hurl.Selector.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
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

        windowManager = WindowManager.Get(this);
        windowManager.IsMaximizable = false;
        windowManager.IsMinimizable = false;
#if DEBUG == false
            windowManager.AppWindow.IsShownInSwitchers = false;
#endif
        windowManager.MinWidth = 500;
        windowManager.MinHeight = 250;

        //window.AppWindow.IsShownInSwitchers = false;
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
    //private void Draggable(object sender, MouseButtonEventArgs e) => DragMove();
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

    private void Linkpreview_Click(object sender, RoutedEventArgs e) => UrlEdit();

    async private void UrlEdit()
    {
        //forcePreventWindowDeactivationEvent = true;
        //var NewUrl = await URLEditor.ShowInputAsync(this, OpenedUri.Value);
        //forcePreventWindowDeactivationEvent = false;

        //if (!string.IsNullOrEmpty(NewUrl))
        //{
        //    OpenedUri.Value = NewUrl;
        //    linkpreview.Content = NewUrl;
        //}
        //else
        //{
        //    OpenedUri.Clear();
        //    linkpreview.Content = "No URL Opened";
        //}

    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        MinimizeWindow();
        Process.Start(Constants.SETTINGS_APP, "--page rulesets");
    }

    private void ClearUriBtnClick(object sender, RoutedEventArgs e)
    {
        //OpenedUri.Clear();
        //linkpreview.Content = "No URL Opened";
    }
}
