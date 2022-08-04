using Hurl.BrowserSelector.Models;
using Hurl.BrowserSelector.Views.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.BrowserSelector.Views
{
    /// <summary>
    /// Interaction logic for BrowsersList.xaml
    /// </summary>
    public partial class BrowsersList : UserControl
    {        
        public BrowsersList()
        {
            InitializeComponent();
        }

        private void BtnArea_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tag = (sender as Border).Tag as Browser;
            Process.Start(tag.ExePath, "https://github.com/u-c-s" + " " + tag.LaunchArgs);

            //if (!string.IsNullOrEmpty(browser.LaunchArgs) && browser.LaunchArgs.Contains("%URL%"))
            //{
            //    var newArg = browser.LaunchArgs.Replace("%URL%", _currentLink.Url);
            //    Process.Start(browser.ExePath, newArg);
            //}
            //else
            //{
            //    Process.Start(browser.ExePath, _currentLink.Url + " " + browser.LaunchArgs);
            //}
            MinimizeWindow();
        }

        private void MinimizeWindow()
        {
            var parent = Window.GetWindow(this);
            parent.WindowState = WindowState.Minimized;
            parent.Hide();
        }
    }
}
