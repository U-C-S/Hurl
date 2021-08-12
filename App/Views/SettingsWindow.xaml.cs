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
