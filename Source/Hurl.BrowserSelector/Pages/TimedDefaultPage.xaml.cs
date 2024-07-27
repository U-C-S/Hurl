using Hurl.BrowserSelector.Controls;
using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Pages.ViewModels;
using Hurl.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            DataContext = new TimedDefaultViewModel();

            LoadBrowsers();
        }

        public void LoadBrowsers()
        {
            foreach (var (browser, i) in (DataContext as TimedDefaultViewModel).Browsers)
            {
                var browserBtn = new TogglableBrowserButton(browser, i, ChangeSelectedItem);
                BrowsersList.Children.Add(browserBtn);
            }
        }

        public int? CurrentSelectedIndex { get; set; }

        public void ChangeSelectedItem(int index, bool isChecked)
        {
            int? prev = CurrentSelectedIndex;
            CurrentSelectedIndex = isChecked ? index : null;

            var browserList = BrowsersList.Children;

            if (prev is int prevInt)
                (browserList[prevInt] as TogglableBrowserButton)!.IsChecked = false;

            if (CurrentSelectedIndex is int i)
                (browserList[i] as TogglableBrowserButton)!.IsChecked = isChecked;
        }

        private void SetBtn_Click(object sender, RoutedEventArgs e)
        {
            var x = TimeBox.Value;
            if (CurrentSelectedIndex is int i)
            {
                TimedBrowserSelect.Create((int)x, SettingsGlobal.Value.Browsers[i]);
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
