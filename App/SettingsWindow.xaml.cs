using Hurl.Constants;
using Hurl.Controls;
using Hurl.Services;
using Hurl.Views;
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;

namespace Hurl
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private Installer InstallerService;

        public SettingsWindow()
        {
            InitializeComponent();
            LoadSystemBrowserList();

            InstallerService = new Installer();

            if (InstallerService.IsDefault) SetDefaultPostExecute();
            if (InstallerService.HasProtocol) ProtocolPostExecute();
        }

        private void SetAsDefualt(object sender, RoutedEventArgs e) => InstallerService.SetDefault();

        private void Protocol_Button(object sender, RoutedEventArgs e)
        {
            bool x = InstallerService.ProtocolRegister();

            if (x) ProtocolPostExecute();
        }

        //Browsers Tab
        public void LoadSystemBrowserList()
        {
            BrowsersList x = GetBrowsers.FromRegistry();

            foreach (BrowserObject i in x)
            {
                if (i.Name != null)
                {
                    var comp = new BrowserStatusComponent
                    {
                        BrowserName = i.Name,
                        BrowserPath = i.ExePath,
                        Img = i.GetIcon,
                        EditEnabled = true,
                        BackColor = "#FFFFDAAD",
                        Margin = new Thickness(0, 4, 0, 0),
                    };
                    //comp.DeleteItem += DeleteBrowser;
                    _ = StackSystemBrowsers.Children.Add(comp);
                }

                if (i.GetIcon == null)
                {
                    System.Windows.Forms.MessageBox.Show("lol");
                }

            }
        }

        //Add browsers
        private void AddBrowser(object sender, RoutedEventArgs e)
        {
            BrowserForm f = new BrowserForm();
            if (f.ShowDialog() == true)
            {
                BrowserObject newBrowser = new BrowserObject()
                {
                    Name = f.BrowserName,
                    ExePath = f.BrowserPath,
                    SourceType = BrowserSourceType.User,
                };
                var comp = new BrowserStatusComponent
                {
                    BrowserName = f.BrowserName,
                    BrowserPath = f.BrowserPath,
                    EditEnabled = true,
                    BackColor = "#FFFFDAAD",
                    Margin = new Thickness(0, 4, 0, 0),
                    Img = newBrowser.GetIcon,
                };
                StackUserBrowsers.Children.Add(comp);
            }
        }

        private void RefreshBrowserList(object sender, RoutedEventArgs e)
        {
            StackSystemBrowsers.Children.Clear();
            LoadSystemBrowserList();
        }

        private void LaunchDebugHurlBtn(object sender, RoutedEventArgs e)
        {
            Process.Start(OtherStrings.APP_LAUNCH_PATH, URLBox.Text);
        }

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
