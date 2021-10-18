using Hurl.Settings.Controls;
using Hurl.Settings.Services;
using Hurl.Settings.Views;
using Hurl.SharedLibraries.Constants;
using Hurl.SharedLibraries.Models;
using Hurl.SharedLibraries.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Hurl.Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Installer InstallerService;

        public MainWindow()
        {
            InitializeComponent();

            InstallerService = new Installer();

            LoadBrowserList();

            if (InstallerService.IsDefault) SetDefaultPostExecute();
            if (InstallerService.HasProtocol) ProtocolPostExecute();
        }

        //Browsers Tab
        private void LoadBrowserList(bool onlyRegistryBrowsers = false)
        {
            List<Browser> x = SettingsFile.LoadNewInstance().SettingsObject.Browsers;

            foreach (Browser i in x)
            {
                var comp = new BrowserStatusComponent(i)
                {
                    Margin = new Thickness(0, 4, 0, 0),
                };
                if (i.SourceType == BrowserSourceType.Registry)
                {
                    _ = StackSystemBrowsers.Children.Add(comp);
                }
                else if (!onlyRegistryBrowsers && i.SourceType == BrowserSourceType.User)
                {
                    _ = StackUserBrowsers.Children.Add(comp);
                }

            }
        }

        private void AddBrowser(object sender, RoutedEventArgs e)
        {
            SettingsFile settingsFile = SettingsFile.LoadNewInstance();

            BrowserForm f = new BrowserForm();
            if (f.ShowDialog() == true)
            {
                Browser newBrowser = new Browser(f.BrowserName, f.BrowserPath)
                {
                    SourceType = BrowserSourceType.User,
                };

                var comp = new BrowserStatusComponent(newBrowser)
                {
                    Margin = new Thickness(0, 4, 0, 0),
                };
                StackUserBrowsers.Children.Add(comp);

                settingsFile.SettingsObject.Browsers.Add(newBrowser);
                settingsFile.Update();
            }
        }

        // Just refresh the Registry browser list
        // since they can be uninstalled at any point and can be known
        private void RefreshBrowserList(object sender, RoutedEventArgs e)
        {
            SettingsFile settingsFile = SettingsFile.LoadNewInstance();

            IEnumerable<Browser> userBrowsers = from b in settingsFile.SettingsObject.Browsers
                                                where b.SourceType == BrowserSourceType.User
                                                select b;
            List<Browser> newRegistryBrowsers = GetBrowsers.FromRegistry();

            settingsFile.SettingsObject.Browsers = userBrowsers.Concat(newRegistryBrowsers).ToList();
            settingsFile.Update();

            StackSystemBrowsers.Children.Clear();
            LoadBrowserList(true);
        }

        private void SetAsDefualt(object sender, RoutedEventArgs e) => InstallerService.SetDefault();

        private void Protocol_Button(object sender, RoutedEventArgs e)
        {
            bool x = InstallerService.ProtocolRegister();
            if (x) ProtocolPostExecute();
        }

        private void LaunchDebugHurlBtn(object sender, RoutedEventArgs e) => Process.Start(OtherStrings.APP_PARENT_DIR + "\\Hurl.exe", URLBox.Text);

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void SetDefaultPostExecute()
        {
            DefaultInfo.Text = "Hurl is currently the default handler for http/https links";
            DefaultSetButton.IsEnabled = false;
        }
        private void ProtocolPostExecute()
        {
            ProtocolInfo.Text = "Protocol is installed and Avaliable through hurl://";
            ProtocolSetButton.IsEnabled = false;
        }
    }
}
