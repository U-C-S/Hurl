using Hurl.BrowserSelector.Converters;
using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.Library.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hurl.BrowserSelector.Controls
{
    /// <summary>
    /// Interaction logic for BrowserButton.xaml
    /// </summary>
    public partial class BrowserButton : UserControl
    {
        public BrowserButton(Browser browser)
        {
            InitializeComponent();
            DataContext = browser;
        }

        private void BrowserButton_Click(object sender, RoutedEventArgs e)
        {
            UriLauncher.Default(UriGlobal.Value, (Browser)DataContext);
            MinimizeWindow();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var alt = ((MenuItem)sender).Tag as AltLaunchParentConverter.AltLaunchParent;
            OpenAltLaunch(alt.AltLaunch, alt.Browser);
            MinimizeWindow();
        }

        private void AdditionalBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var x = Resources["TheCM"] as ContextMenu;
            x.PlacementTarget = btn;
            x.IsOpen = true;
            e.Handled = true;
        }

        private void MinimizeWindow()
        {
            var parent = Window.GetWindow(this);
            parent.WindowState = WindowState.Minimized;
            parent.Hide();
        }

        public static void OpenAltLaunch(AlternateLaunch alt, Browser browser)
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

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            AdditionalBtn.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            AdditionalBtn.Visibility = Visibility.Hidden;
        }

        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            AdditionalBtn.Visibility = Visibility.Visible;
        }

        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            AdditionalBtn.Visibility = Visibility.Hidden;
        }
    }
}
