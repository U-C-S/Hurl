using Hurl.BrowserSelector.Controls;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.State;
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
        var appSettings = Settings.AppSettings;

        InitializeComponent();

        var osbuild = Environment.OSVersion.Version.Build;
        var backtype = appSettings?.BackgroundType;

        if (appSettings?.NoWhiteBorder == true) WindowBorder.BorderThickness = new Thickness(0);
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
        var appSettings = Settings.AppSettings;
        var isRuleCheckSuccess = appSettings.RuleMatching && AutoRulesCheck.Start(data.Url);

        if (!data.IsSecondInstance)
        {
            Width = appSettings?.WindowSize[0] ?? 420;
            Height = appSettings?.WindowSize[1] ?? 210;
        }

        if (data.IsRunAsMin || isRuleCheckSuccess)
        {
            MinimizeWindow();
            Show();
        }
        else
        {
            ShowWindow();
        }

        linkpreview.Content = string.IsNullOrEmpty(OpenedUri.Value) ? "No Url Opened" : OpenedUri.Value;
    }

    public void LoadBrowsers()
    {
        foreach (var browser in Settings.Browsers)
        {
            if (!browser.Hidden)
            {
                var browserBtn = new BrowserButton(browser);
                BrowsersList.Children.Add(browserBtn);
                this.InputBindings.Add(new KeyBinding()
                {
                    Command = browserBtn.Command,
                    Key = (Key)36
                });
            }
        }
    }

    private void Window_KeyEvents(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                MinimizeWindow();
                break;
            case Key.E:
                UrlEdit();
                break;
            case Key.C:
                try
                {
                    Clipboard.SetText(OpenedUri.Value);
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
                new TimeSelectWindow(Settings.Browsers).ShowDialog();
                break;
            default:
                break;
        }

    }

    private void LinkCopyBtnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Clipboard.SetText(OpenedUri.Value);
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
        string? tag = ((MenuItem)sender).Tag as string;
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
        var appSettings = Settings.AppSettings;
        if (!forcePreventWindowDeactivationEvent && appSettings.MinimizeOnFocusLoss) MinimizeWindow();
#endif
    }

    private void PositionWindowUnderTheMouse()
    {
        var appSettings = Settings.AppSettings;

        try
        {
            if (appSettings?.LaunchUnderMouse == true)
            {
                var transform = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformFromDevice;
                if (transform is Matrix t)
                {
                    var mouse = t.Transform(CursorPosition.LimitCursorWithin((int)Width, (int)Height));
                    Left = mouse.X;
                    Top = mouse.Y;

                    Debug.WriteLine($"{Left}�{Top} with screen resolution: {SystemParameters.FullPrimaryScreenWidth}�{SystemParameters.FullPrimaryScreenHeight}");
                }
            }
        }
        catch (Exception) { }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        forcePreventWindowDeactivationEvent = true;
        new TimeSelectWindow(Settings.Browsers).ShowDialog();
        forcePreventWindowDeactivationEvent = false;
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e) => Settings.AdjustWindowSize(e);

    private void Linkpreview_Click(object sender, RoutedEventArgs e) => UrlEdit();

    async private void UrlEdit()
    {
        forcePreventWindowDeactivationEvent = true;
        var NewUrl = await URLEditor.ShowInputAsync(this, OpenedUri.Value);
        forcePreventWindowDeactivationEvent = false;

        if (!string.IsNullOrEmpty(NewUrl))
        {
            OpenedUri.Value = NewUrl;
            linkpreview.Content = NewUrl;
        }
        else
        {
            OpenedUri.Clear();
            linkpreview.Content = "No URL Opened";
        }

    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        MinimizeWindow();
        Process.Start(Constants.SETTINGS_APP, "--page rulesets");
    }

    private void ClearUriBtnClick(object sender, RoutedEventArgs e)
    {
        OpenedUri.Clear();
        linkpreview.Content = "No URL Opened";
    }
}
