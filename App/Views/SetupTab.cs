using Hurl.Constants;
using Hurl.Services;
using System;
using System.Windows;
using System.Windows.Forms;

namespace Hurl.Views
{
    public partial class SettingsWindow : Window
    {
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
