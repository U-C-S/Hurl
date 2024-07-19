using Hurl.BrowserSelector.Controls;
using Hurl.BrowserSelector.Globals;
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
using Hurl.BrowserSelector.Windows;

namespace Hurl.BrowserSelector.Pages
{
    /// <summary>
    /// Interaction logic for SelectPage.xaml
    /// </summary>
    public partial class SelectPage : Page
    {
        private bool forcePreventWindowDeactivationEvent = false;

        public SelectPage()
        {
            InitializeComponent();

            linkpreview.Content = string.IsNullOrEmpty(UriGlobal.Value) ? "No Url Opened" : UriGlobal.Value;
            LoadBrowsers();
        }

        private void LinkCopyBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(UriGlobal.Value);
            }
            catch (Exception err)
            {
                System.Windows.MessageBox.Show(err.Message);
            }
        }

        async private void Linkpreview_Click(object sender, RoutedEventArgs e)
        {
            forcePreventWindowDeactivationEvent = true;
            var parentWindow = Window.GetWindow(this);
            var NewUrl = await URLEditor.ShowInputAsync(parentWindow, UriGlobal.Value);
            if (!string.IsNullOrEmpty(NewUrl))
            {
                UriGlobal.Value = NewUrl;
                ((Button)sender).Content = NewUrl;
                ((Button)sender).ToolTip = NewUrl;
            }

            forcePreventWindowDeactivationEvent = false;
        }

        public void LoadBrowsers()
        {
            foreach (var browser in SettingsGlobal.Value.Browsers)
            {
                var browserBtn = new BrowserButton(browser);
                BrowsersList.Children.Add(browserBtn);
            }
        }
    }



}
