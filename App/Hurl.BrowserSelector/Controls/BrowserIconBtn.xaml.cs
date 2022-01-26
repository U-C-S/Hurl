using Hurl.SharedLibraries.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Hurl.BrowserSelector.Controls
{
    /// <summary>
    /// Interaction logic for BrowserIconBtn.xaml
    /// </summary>
    public partial class BrowserIconBtn : UserControl
    {
        private Browser browser { get; set; }
        private string URL { get; set; }

        public BrowserIconBtn(Browser browser, string URL)
        {
            InitializeComponent();
            this.URL = URL;
            DataContext = this.browser = browser;
        }

        private void OpenIt(object sender, MouseButtonEventArgs e)
        {
            Process.Start(browser.ExePath, URL + " " + browser.LaunchArgs);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var alt = (sender as MenuItem).Tag as AlternateLaunch;
            Process.Start(browser.ExePath, URL + " " + alt.LaunchArgs);
        }
    }
}
