using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace Hurl.BrowserSelector.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    {
        private Settings settings;

        public MainWindow(Settings settings)
        {
            this.settings = settings;

            //if (Environment.OSVersion.Version.Build < 22523 && settings.AppSettings?.UseMica != true && settings.AppSettings?.DisableAcrylic != true)
            //{
            //    AllowsTransparency = true;
            //    Background = new SolidColorBrush(Color.FromArgb(120, 2, 0, 0));
            //}
            //if (Environment.OSVersion.Version.Build < 22523 && settings.AppSettings?.UseMica != true)
            //{
            //    WindowBackdropType = BackgroundType.None;
            //}

            InitializeComponent();

            if (settings.AppSettings?.UseWhiteBorder == false) WindowBorder.BorderThickness = new Thickness(0);
            if (Environment.OSVersion.Version.Build < 22000) WindowBorder.CornerRadius = new CornerRadius(0);

            if (settings.AppSettings?.DisableAcrylic != true)
            {
                if (Environment.OSVersion.Version.Build >= 22000 && settings.AppSettings?.UseMica == true)
                {
                    WindowBackdropType = BackgroundType.Mica;
                }
            }
            else
            {
                WindowBackdropType = BackgroundType.None;
                var c = settings?.AppSettings?.BackgroundRGB;
                var brush = (c != null) ? Color.FromRgb(c[0], c[1], c[2]) : Color.FromRgb(150, 50, 50);
                Background = new SolidColorBrush(brush);
            }
        }

        public void Init(CliArgs data, bool IsTimedSet = false)
        {
            if (data.IsRunAsMin || IsTimedSet)
            {
                Debug.WriteLine("Minimizing--------------------------------");
                MinimizeWindow();
            }
            else
            {
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
        private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start("notepad.exe", Constants.SettingsFilePath);
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

        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrolledval = e.Delta;
            var ui = (ScrollViewer)(sender as StackPanel).Parent;
            // Debug.WriteLine(scrolledval);

            //scroll horizontally on mouse wheel
            if (ui.HorizontalOffset + scrolledval < 0)
            {
                ui.ScrollToHorizontalOffset(0);
            }
            else if (ui.HorizontalOffset + scrolledval > ui.ScrollableWidth)
            {
                ui.ScrollToHorizontalOffset(ui.ScrollableWidth);
            }
            else
            {
                ui.ScrollToHorizontalOffset(ui.HorizontalOffset + scrolledval);
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
    }
}
