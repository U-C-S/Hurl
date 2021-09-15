using Hurl.Constants;
using Hurl.Controls;
using Hurl.Services;
using Hurl.Views;
using System;
using System.Security.Principal;
using System.Windows;
using System.Windows.Forms;

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

            if (InstallerService.isDefault)
            {
                DefaultInfo.Text = "Hurl is currently the default handler for http/https links";
                DefaultSetButton.IsEnabled = false;
            }

            //InstallPathTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Hurl";
            //if (InstallerService.isInstalled)
            //{
            //    InstallInfo.Text = "Hurl is already installed at the following Location.";
            //    InstallButton.IsEnabled = false;
            //}
        }

        //Setup Tab
        //private void InstallPathSelect(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    var dialog = new FolderBrowserDialog
        //    {
        //        Description = "Select the Destination Folder where the Application Files and Settings will be Stored",
        //        SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        //        ShowNewFolderButton = true
        //    };
        //    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        InstallPathTextBox.Text = dialog.SelectedPath;
        //    }
        //}
        //private void Install_Button(object sender, RoutedEventArgs e) => InstallerService.Install(InstallPathTextBox.Text);

        //private void Uninstall_Button(object sender, RoutedEventArgs e) => InstallerService.Uninstall();

        private void SetAsDefualt(object sender, RoutedEventArgs e) => InstallerService.SetDefault();

        private void Protocol_Button(object sender, RoutedEventArgs e) => InstallerService.ProtocolRegister();

        //Browsers Tab
        public void LoadSystemBrowserList()
        {
            GetBrowsers x = GetBrowsers.InitalGetList();

            foreach (BrowserObject i in x)
            {
                if (i.Name != null)
                {
                    var comp = new BrowserStatusComponent
                    {
                        BrowserName = i.Name,
                        BrowserPath = i.ExePath,
                        EditEnabled = true,
                        BackColor = "#FFFFDAAD",
                        Margin = new Thickness(0, 4, 0, 0),
                    };
                    //comp.DeleteItem += DeleteBrowser;
                    _ = StackSystemBrowsers.Children.Add(comp);
                }

            }
        }

        //Add browsers
        private void AddBrowser(object sender, RoutedEventArgs e)
        {
            BrowserForm f = new BrowserForm();
            if (f.ShowDialog() == true)
            {
                var comp = new BrowserStatusComponent
                {
                    BrowserName = f.BrowserName,
                    BrowserPath = f.BrowserPath,
                    EditEnabled = true,
                    BackColor = "#FFFFDAAD",
                    Margin = new Thickness(0, 4, 0, 0),
                };
                StackUserBrowsers.Children.Add(comp);

            }
        }

        private void RefreshBrowserList(object sender, RoutedEventArgs e)
        {
            StackSystemBrowsers.Children.Clear();
            LoadSystemBrowserList();
        }
    }
}
