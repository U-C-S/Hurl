using Hurl.BrowserSelector.Controls;
using Hurl.BrowserSelector.Helpers;
using Hurl.SharedLibraries;
using Hurl.SharedLibraries.Models;
using Hurl.SharedLibraries.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CurrentLink OpenedLink = new("");
        private Settings settings;

        public MainWindow()
        {
            LoadSettings();

            InitializeComponent();

            if (settings?.AppSettings?.DisableAcrylic == false)
                WPFUI.Appearance.Background.Apply(this, WPFUI.Appearance.BackgroundType.Acrylic, true);
            else
            {
                var c = settings?.AppSettings.BackgroundRGB;
                this.Background = new SolidColorBrush(Color.FromRgb(c[0], c[1], c[2]));
            }


            RoundedCorners.Apply(this, () => WindowBorder.CornerRadius = new CornerRadius(0));

            ShowBrowserIcons();
        }

        public void LoadSettings()
        {
            try
            {
                settings = SettingsFile.GetSettings();
            }
            catch (JsonException e)
            {
                MessageBox.Show(e.Message,"ERROR");
                throw e;
            }
            catch (FileNotFoundException)
            {
                var _browsersList = GetBrowsers.FromRegistry();
                settings = SettingsFile.New(_browsersList).SettingsObject;
            }
        }

        private void ShowBrowserIcons()
        {
#if DEBUG
            Stopwatch sw = new();
            sw.Start();
#endif
            List<Browser> LoadableBrowsers = null;
            try
            {
                LoadableBrowsers = GetBrowsers.FromSettingsFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Shutdown();
                return;
            }
#if DEBUG
            sw.Stop();
            Debug.WriteLine("---------" + sw.ElapsedMilliseconds.ToString());
#endif

            foreach (Browser i in LoadableBrowsers)
            {
                _ = stacky.Children.Add(new BrowserIconBtn(i, OpenedLink));
            }
        }

        public void Init(CliArgs data)
        {
            Show();
            if (data.IsSecondInstance)
            {
                this.WindowState = WindowState.Normal;
            }

            var Url = data?.Url ?? string.Empty;

            OpenedLink.Url = Url;
            linkpreview.Text = Url;
        }

        private void Window_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MinimizeWindow();
            }
        }

        private void LinkCopyBtnClick(object sender, RoutedEventArgs e) => Clipboard.SetText(OpenedLink.Url);
        private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start("notepad.exe", Constants.SettingsFilePath);
        private void Draggable(object sender, MouseButtonEventArgs e) => this.DragMove();
        private void CloseBtnClick(object sender, RoutedEventArgs e) => MinimizeWindow();

        private void MinimizeWindow()
        {
            this.WindowState = WindowState.Minimized;
            this.Hide();
        }

        private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string tag = (sender as MenuItem).Tag as string;
            try
            {
                switch (tag)
                {
                    case "open":
                        this.Show();
                        this.WindowState = WindowState.Normal;
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
    }
}
