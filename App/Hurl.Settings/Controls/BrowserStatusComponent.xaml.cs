using Hurl.SharedLibraries.Models;
using Hurl.SharedLibraries.Services;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.Settings.Controls
{
    /// <summary>
    /// Interaction logic for BrowserStatusComponent.xamll
    /// </summary>
    public partial class BrowserStatusComponent : UserControl
    {
        private Browser _browser { get; set; }
        private string BrowserName;

        public BrowserStatusComponent(Browser browser)
        {
            InitializeComponent();
            this.BrowserName = browser.Name;
            this._browser = browser;

            DataContext = this._browser;
        }

        public bool EditEnabled { get; set; } = true;
        //public RoutedEventHandler DeleteItem;

        private void CopyPath(object sender, RoutedEventArgs e) => Clipboard.SetText(_browser.ExePath);

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

        private void ChooseExeBtn(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Select the Path of the EXE",
                Filter = "Application File (*.exe)|*.exe",
            };

            if (dialog.ShowDialog() == true)
            {
                _browser.ExePath = ExePathBox.Text = dialog.FileName;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e) => TheExpander.IsExpanded = false;

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            SettingsFile settings = SettingsFile.LoadNewInstance();
            var BrowserList = settings.SettingsObject.Browsers;

            var i = BrowserList.FindIndex(b => b.Name == BrowserName);
            BrowserList[i] = _browser;
            //DEBUG_BOX.Text =  _browser.ExePath;
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
