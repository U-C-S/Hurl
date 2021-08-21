using Hurl.Browser;
using Hurl.Controls;
using System;
using System.Windows;

namespace Hurl.Views
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
    }
}
