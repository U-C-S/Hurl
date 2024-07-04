using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.Library;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;
using Path = System.IO.Path;

namespace Hurl.BrowserSelector.Windows;

public partial class MainWindow : FluentWindow
{
    private bool forcePreventWindowDeactivationEvent = false;

    public MainWindow()
    {
        var settings = Globals.SettingsGlobal.Value;

        InitializeComponent();

        var osbuild = Environment.OSVersion.Version.Build;
        var backtype = settings.AppSettings?.BackgroundType;

        if (settings.AppSettings?.NoWhiteBorder == true) WindowBorder.BorderThickness = new Thickness(0);
        if (osbuild < 22000) WindowBorder.CornerRadius = new CornerRadius(0);

        if (backtype == "acrylic" && osbuild >= 22523)
        {
            WindowBackdropType = WindowBackdropType.Acrylic;
        }
        else if (backtype == "none" || osbuild < 22000)
        {
            WindowBackdropType = WindowBackdropType.None;
            var brush = Color.FromRgb(150, 50, 50);
            Background = new SolidColorBrush(brush);
        }
        else
        {
            WindowBackdropType = WindowBackdropType.Mica;
        }
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
            Show(); // TODO: try to remove the flashing
            MinimizeWindow();
        }
        else
        {
            Show();
            PositionWindowUnderTheMouse();
            if (data.IsSecondInstance)
            {
                WindowState = WindowState.Normal;
            }
        }

        linkpreview.Content = string.IsNullOrEmpty(UriGlobal.Value) ? "No Url Opened" : UriGlobal.Value;
    }

    async private void Window_KeyEvents(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                MinimizeWindow();
                break;
            case Key.E:
                {
                    var NewUrl = await URLEditor.ShowInputAsync(this, UriGlobal.Value);
                    if (!string.IsNullOrEmpty(NewUrl))
                    {
                        UriGlobal.Value = NewUrl;
                        linkpreview.Content = NewUrl;
                        linkpreview.ToolTip = NewUrl;
                    }

                    break;
                }
            case Key.C:
                try
                {
                    Clipboard.SetText(UriGlobal.Value);
                }
                catch (Exception err)
                {
                    System.Windows.MessageBox.Show(err.Message);
                }
                break;
            case Key.R:
                MinimizeWindow();
                Process.Start(Constants.SETTINGS_APP);
                break;
            case Key.T:
                new TimeSelectWindow(SettingsGlobal.Value.Browsers).ShowDialog();
                break;
            default:
                break;
        }

    }

    private void LinkCopyBtnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Clipboard.SetText(UriGlobal.Value);
        }
        catch (Exception err)
        {
            System.Windows.MessageBox.Show(err.Message);
        }
    }

    private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start("explorer", "\"" + Constants.APP_SETTINGS_MAIN + "\"");
    private void Draggable(object sender, MouseButtonEventArgs e) => this.DragMove();
    private void CloseBtnClick(object sender, RoutedEventArgs e) => MinimizeWindow();

    private void MinimizeWindow()
    {
        this.WindowState = WindowState.Minimized;
        this.Hide();
    }

    public void MaximizeWindow()
    {
        this.Show();
        PositionWindowUnderTheMouse();
        this.WindowState = WindowState.Normal;
    }

    private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var settings = Globals.SettingsGlobal.Value;

        string tag = (sender as Wpf.Ui.Controls.MenuItem).Tag as string;
        try
        {
            switch (tag)
            {
                case "open":
                    MaximizeWindow();
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

    private void NotifyIcon_LeftClick(object sender, RoutedEventArgs e) => MaximizeWindow();

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

            Debug.WriteLine($"{Left}x{Top} while screen res: {SystemParameters.FullPrimaryScreenWidth}x{SystemParameters.FullPrimaryScreenHeight}");
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        forcePreventWindowDeactivationEvent = true;
        new TimeSelectWindow(Globals.SettingsGlobal.Value.Browsers).ShowDialog();
        forcePreventWindowDeactivationEvent = false;
    }

    private void UiWindow_SizeChanged(object sender, SizeChangedEventArgs e) => Globals.SettingsGlobal.AdjustWindowSize(e);

    async private void linkpreview_Click(object sender, RoutedEventArgs e)
    {
        forcePreventWindowDeactivationEvent = true;

        var NewUrl = await URLEditor.ShowInputAsync(this, UriGlobal.Value);
        if (!string.IsNullOrEmpty(NewUrl))
        {
            UriGlobal.Value = NewUrl;
            (sender as Wpf.Ui.Controls.Button).Content = NewUrl;
            (sender as Wpf.Ui.Controls.Button).ToolTip = NewUrl;
        }

        forcePreventWindowDeactivationEvent = false;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        MinimizeWindow();
        Process.Start(Constants.SETTINGS_APP);
    }
}

