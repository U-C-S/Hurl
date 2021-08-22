using Hurl.Constants;
using Hurl.Controls;
using Hurl.Models;
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
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;

            LoadSystemBrowserList();
            InstallPathTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Hurl";

            // Future: Maybe just dont register the protocol, instead of preventing the user from installing
            if (IsAdministrator())
            {
                InstallButton.IsEnabled = true;
            }
            else
            {
                InstallInfo.Text = "Run the Application as Adminstrator to Install";
                InstallInfo.FontWeight = FontWeights.Bold;
            }
        }

        public void LoadSystemBrowserList()
        {
            BList x = BList.InitalGetList();

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


        public string SetupLog = "";

        //Move this to Constants class
        private static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void InstallPathSelect(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Select the Destination Folder where the Application Files and Settings will be Stored",
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InstallPathTextBox.Text = dialog.SelectedPath;
            }
        }

        private void Install_Button(object sender, RoutedEventArgs e)
        {
            //Logger x = new Logger(SetupLog);
            new Setup().Install(InstallPathTextBox.Text, LogTextBox);
            System.Windows.MessageBox.Show("Installed with Root: " + Environment.GetCommandLineArgs()[0]);
        }

        private void Uninstall_Button(object sender, RoutedEventArgs e)
        {
            Setup.Uninstall();
            System.Windows.MessageBox.Show("Uninstalled from Registry");

        }
    }
}
