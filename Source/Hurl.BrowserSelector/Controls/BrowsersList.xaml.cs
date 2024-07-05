using Hurl.BrowserSelector.Converters;
using Hurl.BrowserSelector.Globals;
using Hurl.Library.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hurl.BrowserSelector.Controls
{
    /// <summary>
    /// Interaction logic for BrowsersList.xaml
    /// </summary>
    public partial class BrowsersList : UserControl
    {
        public BrowsersList()
        {
            DataContext = Globals.SettingsGlobal.GetBrowsers();
            InitializeComponent();
            //Loaded += (s, e) =>
            //{
            //    var parent = Window.GetWindow(this);
            //    parent.PreviewKeyUp += OnPreviewKeyUp;
            //};
            //Unloaded += (s, e) =>
            //{
            //    var parent = Window.GetWindow(this);
            //    parent.PreviewKeyUp -= OnPreviewKeyUp;
            //};
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            var val = ((int)e.Key) - ((int)Key.D1);

            if (val >= 0 && val <= 9 && SettingsGlobal.Value.Browsers.Count >= val + 1)
            {
                OpenLink(SettingsGlobal.Value.Browsers[val]);
            }
            // TODO : Add support for Ctrl + 1-9 and Custom Hotkeys and Fav
            //if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) { }
        }

        private void Button(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tag = ((Button)sender).Tag as Browser;
            OpenLink(tag);
            MinimizeWindow();
        }

        private void MinimizeWindow()
        {
            var parent = Window.GetWindow(this);
            parent.WindowState = WindowState.Minimized;
            parent.Hide();
        }

        private void AdditionalBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var x = Resources["TheCM"] as ContextMenu;
            x.PlacementTarget = btn;
            x.IsOpen = true;
            e.Handled = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var alt = (sender as MenuItem).Tag as AltLaunchParentConverter.AltLaunchParent;
            OpenAltLaunch(alt.AltLaunch, alt.Browser);
            MinimizeWindow();
        }

        public void OpenLink(Browser clickedbrowser)
        {
            var browser = clickedbrowser;
            var Link = UriGlobal.Value;
            //Process.Start(browser.ExePath, "https://github.com/u-c-s" + " " + browser.LaunchArgs);

            if (!string.IsNullOrEmpty(browser.LaunchArgs) && browser.LaunchArgs.Contains("%URL%"))
            {
                var newArg = browser.LaunchArgs.Replace("%URL%", Link);
                Process.Start(browser.ExePath, newArg);
            }
            else
            {
                Process.Start(browser.ExePath, Link + " " + browser.LaunchArgs);
            }
        }

        public void OpenAltLaunch(AlternateLaunch alt, Browser browser)
        {
            if (alt.LaunchArgs.Contains("%URL%"))
            {
                Process.Start(browser.ExePath, alt.LaunchArgs.Replace("%URL%", UriGlobal.Value));
            }
            else
            {
                Process.Start(browser.ExePath, UriGlobal.Value + " " + alt.LaunchArgs);
            }
        }
    }
}
