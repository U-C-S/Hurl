using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Views.ViewModels;
using Hurl.Library;
using Hurl.Library.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Path = System.IO.Path;

namespace Hurl.BrowserSelector.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    {
        private Settings settings
        {
            get
            {
                return (DataContext as MainViewModel).settings;
            }
        }

        public MainWindow(Settings settings)
        {
            InitializeComponent();

            var osbuild = Environment.OSVersion.Version.Build;
            var backtype = settings.AppSettings?.BackgroundType;
            var disableAcrylic = settings.AppSettings?.DisableAcrylic;

            if (settings.AppSettings?.UseWhiteBorder == false) WindowBorder.BorderThickness = new Thickness(0);
            if (osbuild < 22000) WindowBorder.CornerRadius = new CornerRadius(0);

            if (backtype == "acrylic" && osbuild >= 22523)
            {
                WindowBackdropType = BackgroundType.Acrylic;
            }
            else if (backtype == "none" || disableAcrylic == true || osbuild < 22000)
            {
                WindowBackdropType = BackgroundType.None;
                var c = settings?.AppSettings?.BackgroundRGB;
                var brush = (c != null) ? Color.FromRgb(c[0], c[1], c[2]) : Color.FromRgb(150, 50, 50);
                Background = new SolidColorBrush(brush);
            }
            else
            {
                WindowBackdropType = BackgroundType.Mica;
            }
        }

        public void Init(CliArgs data)
        {
            if (data.IsRunAsMin)
            {
                Debug.WriteLine("Minimizing--------------------------------");
                MinimizeWindow();
            }
            else
            {
                if (!data.IsSecondInstance)
                {
                    try
                    {

                        if (settings.AutoRules != null)
                        {
                            var x = AutoRulesCheck.CheckAndRun(data.Url, settings.AutoRules);
                            if (x) return;
                        }

                        Width = settings.AppSettings.WindowSize[0];
                        Height = settings.AppSettings.WindowSize[1];
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        throw;
                    }
                }
                Show();
                PositionWindowUnderTheMouse();
                if (data.IsSecondInstance)
                {
                    this.WindowState = WindowState.Normal;
                }
            }

            linkpreview.Text = string.IsNullOrEmpty(CurrentLink.Value) ? "No Url Opened" : CurrentLink.Value;
        }

        private void Window_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MinimizeWindow();
            }
            //var keyInt = (int)e.Key;
            //if(keyInt >= 35 && keyInt <= 43)
            //{
            //}
        }

        private void LinkCopyBtnClick(object sender, RoutedEventArgs e) => Clipboard.SetText(CurrentLink.Value);
        private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start("notepad.exe", Constants.APP_SETTINGS_MAIN);
        private void Draggable(object sender, MouseButtonEventArgs e) => this.DragMove();
        private void CloseBtnClick(object sender, RoutedEventArgs e) => MinimizeWindow();

        private void MinimizeWindow()
        {
            this.WindowState = WindowState.Minimized;
            this.Hide();
        }

        private void MaximizeWindow()
        {
            this.Show();
            PositionWindowUnderTheMouse();
            this.WindowState = WindowState.Normal;
        }

        private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string tag = (sender as MenuItem).Tag as string;
            try
            {
                switch (tag)
                {
                    case "open":
                        MaximizeWindow();
                        break;
                    case "timed":
                        new TimeSelectWindow(settings.Browsers).ShowDialog();
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
                MessageBox.Show(err.Message);
            }
        }

        private void NotifyIcon_LeftClick(object sender, RoutedEventArgs e) => MaximizeWindow();

        private void Window_Deactivated(object sender, EventArgs e) => MinimizeWindow();

        private void PositionWindowUnderTheMouse()
        {
            if (settings.AppSettings != null && settings.AppSettings.LaunchUnderMouse)
            {
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var mouse = transform.Transform(CursorPosition.LimitCursorWithin((int)Width, (int)Height));
                Left = mouse.X;
                Top = mouse.Y;

                Debug.WriteLine($"{Left}x{Top} while screen res: {SystemParameters.FullPrimaryScreenWidth}x{SystemParameters.FullPrimaryScreenHeight}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) => new TimeSelectWindow(settings.Browsers).ShowDialog();

        private void UiWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width != 0)
            {
                settings.AppSettings.WindowSize = new int[] { (int)e.NewSize.Width, (int)e.NewSize.Height };
                JsonOperations.FromModelToJson(settings, Constants.APP_SETTINGS_MAIN);
            }
        }
    }
}
