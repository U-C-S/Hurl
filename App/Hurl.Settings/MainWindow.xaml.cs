using Hurl.Settings.Controls;
using Hurl.Settings.Services;
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
                var comp = new BrowserComponent(i)
                {
                    Margin = new Thickness(0, 4, 0, 0),
                };
                _ = StackSystemBrowsers.Children.Add(comp);

            }
        }

        private void AddBrowser(object sender, RoutedEventArgs e)
        {
            SettingsFile settingsFile = SettingsFile.LoadNewInstance();

            Browser newBrowser = new Browser("Empty New Browser", null)
            {
                SourceType = BrowserSourceType.User,
            };

            var comp = new BrowserComponent(newBrowser)
            {
                Margin = new Thickness(0, 4, 0, 0),
            };
            StackSystemBrowsers.Children.Add(comp);

            settingsFile.SettingsObject.Browsers.Add(newBrowser);
            settingsFile.Update();
        }

        // Just refresh the Registry browser list
        // since they can be uninstalled at any point and can be known
        private void RefreshBrowserList(object sender, RoutedEventArgs e)
        {
            SettingsFile settingsFile = SettingsFile.LoadNewInstance();

            List<Browser> EmptyBrowserList = new List<Browser>();

            List<Browser> newRegistryBrowsers = GetBrowsers.FromRegistry();

            List<Browser> registryBrowsers = (from b in settingsFile.SettingsObject.Browsers
                                                    where b.SourceType == BrowserSourceType.Registry
                                                    select b).ToList();
            var backuplist = new List<Browser>(registryBrowsers);

            foreach (Browser nb in newRegistryBrowsers)
            {
                bool added = false;
                foreach (Browser cb in registryBrowsers)
                {
                    if (!added && nb.ExePath == cb.ExePath)
                    {
                        added = true;
                        EmptyBrowserList.Add(cb);

                        var booll = backuplist.Remove(cb);
                        Debug.WriteLine(booll);
                        continue;
                    }
                }

                if (!added)
                {
                    EmptyBrowserList.Add(nb);
                }
            }

            //now make all the stuff in backuplist as hidden
            foreach (var x in backuplist)
            {
                x.Hidden = true;
                Debug.WriteLine(x.Name);
            }

            IEnumerable<Browser> userBrowsers = from b in settingsFile.SettingsObject.Browsers
                                                where b.SourceType == BrowserSourceType.User
                                                select b;

            settingsFile.SettingsObject.Browsers = userBrowsers.Concat(EmptyBrowserList.Concat(backuplist)).ToList();
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
