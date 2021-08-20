using Hurl.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Hurl.Views
{
    public partial class SettingsWindow : Window
    {
        private void InstallPathSelect(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Time to select a folder",
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
            new Setup().Install(InstallPathTextBox.Text);
            System.Windows.MessageBox.Show("Installed with Root: " + Environment.GetCommandLineArgs()[0]);
        }

        private void Uninstall_Button(object sender, RoutedEventArgs e)
        {
            Setup.Uninstall();
            System.Windows.MessageBox.Show("Uninstalled from Registry");

        }
    }
}
