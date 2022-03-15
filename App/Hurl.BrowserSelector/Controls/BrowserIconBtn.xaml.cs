using Hurl.SharedLibraries.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hurl.BrowserSelector.Controls
{
    /// <summary>
    /// Interaction logic for BrowserIconBtn.xaml
    /// </summary>
    public partial class BrowserIconBtn : UserControl
    {
        private Browser browser { get; set; }
        private CurrentLink _currentLink { get; set; }

        public BrowserIconBtn(Browser browser, CurrentLink URL)
        {
            InitializeComponent();
            this._currentLink = URL;
            DataContext = this.browser = browser;
        }

        private void OpenIt(object sender, MouseButtonEventArgs e)
        {
            Process.Start(browser.ExePath, _currentLink.Url + " " + browser.LaunchArgs);
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var alt = (sender as MenuItem).Tag as AlternateLaunch;
            Process.Start(browser.ExePath, _currentLink.Url + " " + alt.LaunchArgs);
            //Application.Current.Shutdown();
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }
    }
}
