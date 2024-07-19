using Hurl.BrowserSelector.Controls;
using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.Library.Models;
using System;
using System.Collections.Generic;
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

namespace Hurl.BrowserSelector.Pages
{
    /// <summary>
    /// Interaction logic for TimedDefaultPage.xaml
    /// </summary>
    public partial class TimedDefaultPage : Page
    {
        public TimedDefaultPage()
        {
            InitializeComponent();

            LoadBrowsers();
        }

        public void LoadBrowsers()
        {
            foreach (var browser in SettingsGlobal.Value.Browsers)
            {
                var browserBtn = new TogglableBrowserButton(browser);
                BrowsersList.Children.Add(browserBtn);
            }
        }

        private void SetBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var x = TimeBox.Value;
            var y = 0;

            if (y != -1)
            {
                //TimedBrowserSelect.Create((int)x, browsers[y]);
            }
        }
    }


    public enum PAGE_NAV_STATE
    {
        Choose,
        Active,
        Expired,
    }
}
