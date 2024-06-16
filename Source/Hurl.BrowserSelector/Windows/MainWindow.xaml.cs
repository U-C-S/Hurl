using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.Library;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Path = System.IO.Path;

namespace Hurl.BrowserSelector.Windows;

public partial class MainWindow : FluentWindow
{
    public MainWindow()
    {
        _ = Globals.SettingsGlobal.Value;
        InitializeComponent();
        SystemThemeWatcher.Watch(this);
    }

    public void Init(CliArgs data)
    {
        var settings = SettingsGlobal.Value;
        var isRuleCheckSuccess = AutoRulesCheck.Start(data.Url);

        if (!data.IsSecondInstance)
        {
            Width = settings?.AppSettings?.WindowSize[0] ?? 420;
            Height = settings?.AppSettings?.WindowSize[1] ?? 210;
        }

        if (data.IsRunAsMin || isRuleCheckSuccess)
        {
            MinimizeWindow();
            Show();
        }
        else
        {
            PositionWindowUnderTheMouse();
            Show();
            if (data.IsSecondInstance)
            {
                WindowState = WindowState.Normal;
            }
        }

        linkpreview.Text = string.IsNullOrEmpty(UriGlobal.Value) ? string.Empty : UriGlobal.Value;
    }

    //private void Window_KeyEvents(object sender, KeyEventArgs e)
    //{
    //    switch (e.Key)
    //    {
    //        case Key.Escape:
    //            MinimizeWindow();
    //            break;
    //        case Key.R:
    //            MinimizeWindow();
    //            Process.Start(Constants.SETTINGS_APP);
    //            break;
    //        case Key.T:
    //            new TimeSelectWindow(SettingsGlobal.Value.Browsers).ShowDialog();
    //            break;
    //        default:
    //            break;
    //    }
    //}

    private void SettingsBtn_Click(object sender, RoutedEventArgs e) => Process.Start("explorer", "\"" + Constants.APP_SETTINGS_MAIN + "\"");
    private void Draggable(object sender, MouseButtonEventArgs e) => DragMove();
    private void CloseBtn_Click(object sender, RoutedEventArgs e) => MinimizeWindow();

    private void MinimizeWindow()
    {
        WindowState = WindowState.Minimized;
        Hide();
    }

    public void ShowWindow()
    {
        PositionWindowUnderTheMouse();
        Show();
        WindowState = WindowState.Normal;
    }

    private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        _ = Globals.SettingsGlobal.Value;

        string? tag = ((MenuItem)sender).Tag as string;
        try
        {
            switch (tag)
            {
                case "open":
                    ShowWindow();
                    break;
                case "settings":
                    Process.Start("explorer", "\"" + Constants.APP_SETTINGS_MAIN + "\"");
                    break;
                case "exit":
                    Application.Current.Shutdown();
                    break;
                case "reload":
                    var AppPath = Path.Combine(AppContext.BaseDirectory, "Hurl.exe");
                    Process.Start(AppPath);
                    Application.Current.Shutdown();
                    break;
                default:
                    break;
            }
        }
        catch (Exception err)
        {
            System.Windows.MessageBox.Show(err.Message);
        }
    }

    private void NotifyIcon_LeftClick(object sender, RoutedEventArgs e) => ShowWindow();

    private void Window_Deactivated(object sender, EventArgs e)
    {
#if DEBUG
        // No minimize on debug when not in focus
#else
            if (!forcePreventWindowDeactivationEvent) MinimizeWindow();
#endif
    }

    private void PositionWindowUnderTheMouse()
    {
        var settings = Globals.SettingsGlobal.Value;

        if (settings.AppSettings != null && settings.AppSettings.LaunchUnderMouse)
        {
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            var mouse = transform.Transform(CursorPosition.LimitCursorWithin((int)Width, (int)Height));
            Left = mouse.X;
            Top = mouse.Y;

            Debug.WriteLine($"{Left}×{Top} with screen resolution: {SystemParameters.FullPrimaryScreenWidth}×{SystemParameters.FullPrimaryScreenHeight}");
        }
    }

    private void BtnTimed_Click(object sender, RoutedEventArgs e)
    {
        new TimeSelectWindow(Globals.SettingsGlobal.Value.Browsers).ShowDialog();
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e) => Globals.SettingsGlobal.AdjustWindowSize(e);

    private void ManageBtn_Click(object sender, RoutedEventArgs e)
    {
        MinimizeWindow();
        Process.Start(Constants.SETTINGS_APP);
    }

    private void MinimizeWindow(TitleBar sender, RoutedEventArgs args)
    {
        WindowState = WindowState.Minimized;
        Hide();
    }
}
