using Hurl.Settings.Views;
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
        public BrowserStatusComponent()
        {
            InitializeComponent();

            DataContext = this;
        }

        public string BrowserName { get; set; }
        public string BrowserPath { get; set; }
        public bool EditEnabled { get; set; } = true;
        public ImageSource Img { get; set; }
        //public RoutedEventHandler DeleteItem;

        private void CopyPath(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(BrowserPath);
        }

        private void OpenExe(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(BrowserPath);
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

            var mod = BrowserList.Find(b => b.Name == BrowserName);
            mod.Name = BrowserNameTextBlock.Text = BrowserName;
            mod.ExePath = BrowserPath = BrowserPath;

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
