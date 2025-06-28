using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.ViewModels;
using Hurl.Library;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls;
using Path = System.IO.Path;

namespace Hurl.BrowserSelector.Windows;

public partial class MainWindow : FluentWindow
{
    private bool forcePreventWindowDeactivationEvent = false;

    private readonly SelectorWindowViewModel viewModel;

    public MainWindow()
    {
        DataContext = viewModel = App.AppHost.Services.GetRequiredService<SelectorWindowViewModel>();
        var appSettings = viewModel.OtherSettings;

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

        Width = appSettings?.WindowSize[0] ?? 420;
        Height = appSettings?.WindowSize[1] ?? 210;
    }

    public void Init(CliArgs data)
    {
        var appSettings = viewModel.OtherSettings;
        var ruleCheck = new AutoRulesCheck(viewModel.Url, viewModel.Rulesets, viewModel.Browsers);
        var isRuleCheckSuccess = appSettings.RuleMatching && ruleCheck.Start();

        if (data.IsRunAsMin || isRuleCheckSuccess)
        {
            Show();
            MinimizeWindow();
            ruleCheck.LaunchIfMatch();
        }
        else
        {
            ShowWindow();
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
                    Clipboard.SetText(viewModel.Url);
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
                new TimeSelectWindow(viewModel.Browsers.ToList()).ShowDialog();
                break;
            default:
                break;
        }

    }

    private void LinkCopyBtnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Clipboard.SetText(viewModel.Url);
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
        var appSettings = viewModel.OtherSettings;
        if (!forcePreventWindowDeactivationEvent && appSettings.MinimizeOnFocusLoss) MinimizeWindow();
#endif
    }

    private void PositionWindowUnderTheMouse()
    {
        var appSettings = viewModel.OtherSettings;

        try
        {
            if (appSettings?.LaunchUnderMouse == true)
            {
                var dpiScale = VisualTreeHelper.GetDpi(this);
                var width = (appSettings?.WindowSize[0] ?? 420) * dpiScale.DpiScaleX;
                var height = (appSettings?.WindowSize[1] ?? 210) * dpiScale.DpiScaleY;
                var position = CursorPosition.LimitCursorWithin((int)width, (int)height);
                Left = position.X / dpiScale.DpiScaleX;
                Top = position.Y / dpiScale.DpiScaleY;
            }
        }
        catch (Exception) { }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        forcePreventWindowDeactivationEvent = true;
        new TimeSelectWindow(viewModel.Browsers.ToList()).ShowDialog();
        forcePreventWindowDeactivationEvent = false;
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {

    }

    private void Linkpreview_Click(object sender, RoutedEventArgs e) => UrlEdit();

    async private void UrlEdit()
    {
        forcePreventWindowDeactivationEvent = true;
        var NewUrl = await URLEditor.ShowInputAsync(this, viewModel.Url);
        forcePreventWindowDeactivationEvent = false;

        viewModel.Url = NewUrl;
        linkpreview.Content = string.IsNullOrEmpty(NewUrl) ? "No URL Opened" : NewUrl;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        MinimizeWindow();
        Process.Start(Constants.SETTINGS_APP, "--page rulesets");
    }

    private void ClearUriBtnClick(object sender, RoutedEventArgs e)
    {
        viewModel.Url = string.Empty;
        linkpreview.Content = "No URL Opened";
    }
}
