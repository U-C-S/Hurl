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
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;

            LoadSystemBrowserList();
            InstallPathTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Hurl";
        }

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


        public string SetupLog = "";

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
            new Installer(LogTextBox)
                .Install(InstallPathTextBox.Text);
            // System.Windows.MessageBox.Show("Installed with Root: " + Environment.GetCommandLineArgs()[0]);
        }

        private void Uninstall_Button(object sender, RoutedEventArgs e)
        {
            new Installer().Uninstall();
            //System.Windows.MessageBox.Show("Uninstalled from Registry");
        }

        //Add browsers
        private void Button_Click(object sender, RoutedEventArgs e)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StackSystemBrowsers.Children.Clear();
            LoadSystemBrowserList();
        }
    }
}
