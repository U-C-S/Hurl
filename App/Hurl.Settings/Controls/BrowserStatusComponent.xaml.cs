using Hurl.Settings.Views;
using Hurl.SharedLibraries.Models;
using Hurl.SharedLibraries.Services;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hurl.Settings.Controls
{
    /// <summary>
    /// Interaction logic for BrowserStatusComponent.xaml
    /// </summary>
    public partial class BrowserStatusComponent : UserControl
    {
        private Browser _browser { get; set; }
        private string BrowserName;

        public BrowserStatusComponent(Browser browser)
        {
            InitializeComponent();
            BrowserName = browser.Name;
            DataContext = this._browser = browser;
        }

        public bool EditEnabled { get; set; } = true;
        //public RoutedEventHandler DeleteItem;

        private void CopyPath(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_browser.ExePath);
        }

        private void OpenExe(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(_browser.ExePath, _browser.LaunchArgs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TextBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Select the Path of the EXE",
                Filter = "Application File (*.exe)|*.exe",
            };

            if (dialog.ShowDialog() == true)
            {
                ExePathBox.Text = dialog.FileName;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            //DialogResult = false;
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            SettingsFile settings = SettingsFile.LoadNewInstance();
            var BrowserList = settings.SettingsObject.Browsers;

            var i = BrowserList.FindIndex(b => b.Name == BrowserName);
            BrowserList[i] = _browser;
            BrowserName = _browser.Name;

            settings.Update();
        }

        private void RemoveBtn(object sender, RoutedEventArgs e)
        {
            SettingsFile settings = SettingsFile.LoadNewInstance();
            var BrowserList = settings.SettingsObject.Browsers;

            _ = BrowserList.Remove(BrowserList.Find(b => b.Name == BrowserName));
            ((StackPanel)Parent).Children.Remove(this);

            settings.Update();
        }
    }
}
