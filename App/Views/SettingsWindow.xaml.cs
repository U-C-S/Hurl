using Hurl.Browser;
using System;
using System.Windows;
using System.Windows.Controls;

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

            LoadSystemBrowserList();
        }

        public void LoadSystemBrowserList()
        {
            BList x = BList.InitalGetList();

            foreach (BrowserObject i in x)
            {
                if (i.Name != null)
                {
                    TextBlock text = new TextBlock()
                    {
                        Padding = new Thickness(2),
                        Text = $"{i.Name} -- {i.ExePath}"
                    };
                    _ = StackSystemBrowsers.Children.Add(text);
                }

            }
        }

        private void Install_Button(object sender, RoutedEventArgs e)
        {
            Constants.Setup.Install();
            MessageBox.Show("Installed with Root: " + Environment.GetCommandLineArgs()[0]);
        }

        private void Uninstall_Button(object sender, RoutedEventArgs e)
        {
            Constants.Setup.Uninstall();
            MessageBox.Show("Uninstalled from Registry");

        }
    }
}
