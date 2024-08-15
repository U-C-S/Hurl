using Hurl.BrowserSelector.Controls;
using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.Library;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls;
using Path = System.IO.Path;

namespace Hurl.BrowserSelector.Windows;

public partial class MainWindow : FluentWindow
{
    private bool forcePreventWindowDeactivationEvent = false;

    public MainWindow()
    {
        var settings = SettingsGlobal.Value;

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

        LoadBrowsers();
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
            if (data.IsSecondInstance)
            {
                WindowState = WindowState.Normal;
            }
            Show();
        }

        linkpreview.Content = string.IsNullOrEmpty(UriGlobal.Value) ? "No Url Opened" : UriGlobal.Value;
    }

    public void LoadBrowsers()
    {
        foreach (var browser in SettingsGlobal.Value.Browsers)
        {
            var browserBtn = new BrowserButton(browser);
            BrowsersList.Children.Add(browserBtn);
        }
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
                Process.Start(Constants.SETTINGS_APP, "--page rulesets");
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

    private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start(Constants.SETTINGS_APP, "--page settings");
    private void Draggable(object sender, MouseButtonEventArgs e) => DragMove();
    private void CloseBtnClick(object sender, RoutedEventArgs e) => MinimizeWindow();

    private void MinimizeWindow()
    {
        WindowState = WindowState.Minimized;
        Hide();
    }

    public void ShowWindow()
    {
        Show();
        PositionWindowUnderTheMouse();
        WindowState = WindowState.Normal;
    }

    private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        _ = SettingsGlobal.Value;

        string tag = ((MenuItem)sender).Tag as string;
        try
        {
            switch (tag)
            {
                case "open":
                    ShowWindow();
                    break;
                case "settings":
                    Process.Start(Constants.SETTINGS_APP, "--page settings");
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
        var settings = SettingsGlobal.Value;

        try
        {
            if (settings.AppSettings != null && settings.AppSettings.LaunchUnderMouse)
            {
                var transform = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformFromDevice;
                if (transform is Matrix t)
                {
                    var mouse = t.Transform(CursorPosition.LimitCursorWithin((int)Width, (int)Height));
                    Left = mouse.X;
                    Top = mouse.Y;

                    Debug.WriteLine($"{Left}×{Top} with screen resolution: {SystemParameters.FullPrimaryScreenWidth}×{SystemParameters.FullPrimaryScreenHeight}");
                }
            }
        }
        catch (Exception _) { }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        forcePreventWindowDeactivationEvent = true;
        new TimeSelectWindow(SettingsGlobal.Value.Browsers).ShowDialog();
        forcePreventWindowDeactivationEvent = false;
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e) => SettingsGlobal.AdjustWindowSize(e);

    async private void Linkpreview_Click(object sender, RoutedEventArgs e)
    {
        forcePreventWindowDeactivationEvent = true;

        var NewUrl = await URLEditor.ShowInputAsync(this, UriGlobal.Value);
        if (!string.IsNullOrEmpty(NewUrl))
        {
            UriGlobal.Value = NewUrl;
            ((Button)sender).Content = NewUrl;
            ((Button)sender).ToolTip = NewUrl;
        }

        forcePreventWindowDeactivationEvent = false;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        MinimizeWindow();
        Process.Start(Constants.SETTINGS_APP, "--page rulesets");
    }
}
