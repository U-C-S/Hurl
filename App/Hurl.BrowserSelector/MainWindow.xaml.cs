using Hurl.BrowserSelector.Controls;
using Hurl.SharedLibraries.Constants;
using Hurl.SharedLibraries.Models;
using Hurl.SharedLibraries.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shell;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string OpenedLink;

        public MainWindow(string URL)
        {
            WPFUI.Background.Manager.Apply(WPFUI.Background.BackgroundType.Acrylic, this);

            InitializeComponent();

            if (Environment.OSVersion.Version.Build < 20000)
            {
                WindowBorder.CornerRadius = new CornerRadius(0);
            }
            else
            {
                // For Rounded Window Corners and Shadows
                // src : https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/apply-rounded-corners#example-1---rounding-an-apps-main-window-in-c---wpf
                var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
                DwmSetWindowAttribute(
                    new WindowInteropHelper(GetWindow(this)).EnsureHandle(),
                    DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
                    ref preference,
                    sizeof(uint)
                );
            }

            WindowChrome.SetWindowChrome(this, new WindowChrome()
            {
                CaptionHeight = 1
            });

            linkpreview.Text = OpenedLink = URL;
#if DEBUG
            Stopwatch sw = new();
            sw.Start();
#endif
            //var y = from z in x where z.ExePath is not null and z.Hidden is false select z;
            IEnumerable<Browser> LoadableBrowsers = from b in SettingsFile.LoadNewInstance().SettingsObject.Browsers
                                                    where b.Name != null && b.ExePath != null && b.Hidden != true
                                                    select b;
#if DEBUG
            sw.Stop();
            Debug.WriteLine("---------" + sw.ElapsedMilliseconds.ToString());
#endif
            foreach (Browser i in LoadableBrowsers)
            {
                BrowserIconBtn browserUC = new BrowserIconBtn(i, OpenedLink);

                var separator = new System.Windows.Shapes.Rectangle()
                {
                    Width = 2,
                    Height = 40,
                    Margin = new Thickness(3, 0, 3, 20),
                    Fill = System.Windows.Media.Brushes.AliceBlue
                };

                _ = stacky.Children.Add(browserUC);
                //_ = stacky.Children.Add(separator);
            }

            //stacky.Children.RemoveAt(stacky.Children.Count - 1);
        }

        private void Window_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void LinkCopyBtnClick(object sender, RoutedEventArgs e) => Clipboard.SetText(OpenedLink);
        private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start("notepad.exe", MetaStrings.SettingsFilePath);
        private void Draggable(object sender, MouseButtonEventArgs e) => this.DragMove();
        private void CloseBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            //this.Close();
        }

        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_WINDOW_CORNER_PREFERENCE = 33
        }

        // The DWM_WINDOW_CORNER_PREFERENCE enum for DwmSetWindowAttribute's third parameter, which tells the function
        // what value of the enum to set.
        public enum DWM_WINDOW_CORNER_PREFERENCE
        {
            DWMWCP_DEFAULT = 0,
            DWMWCP_DONOTROUND = 1,
            DWMWCP_ROUND = 2,
            DWMWCP_ROUNDSMALL = 3
        }

        // Import dwmapi.dll and define DwmSetWindowAttribute in C# corresponding to the native function.
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern long DwmSetWindowAttribute(IntPtr hwnd,
                                                         DWMWINDOWATTRIBUTE attribute,
                                                         ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
                                                         uint cbAttribute);
    }
}
