using Hurl.Library.Models;
using Hurl.Library;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hurl.Settings.Pages
{
    /// <summary>
    /// Interaction logic for BrowsersEdit.xaml
    /// </summary>
    public partial class BrowsersEdit : UserControl
    {
        public BrowsersEdit()
        {
            InitializeComponent();
            this.DataContext = Globals.SettingsGlobal.GetBrowsers();
        }

        private void CopyPath(object sender, RoutedEventArgs e)
        {
            //Clipboard.SetText(_browser.ExePath);
        }

        private void OpenExe(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Process.Start(_browser.ExePath, _browser.LaunchArgs);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void ChooseExeBtn(object sender, RoutedEventArgs e)
        {
            //var dialog = new OpenFileDialog()
            //{
            //    Title = "Select the Path of the EXE",
            //    Filter = "Application File (*.exe)|*.exe",
            //};

            //if (dialog.ShowDialog() == true)
            //{
            //    _browser.ExePath = ExePathBox.Text = dialog.FileName;
            //}
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            //((StackPanel)Parent).Children.Remove(this);
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
        //    SettingsFile settings = SettingsFile.LoadNewInstance();
        //    var BrowserList = settings.SettingsObject.Browsers;

        //    var i = BrowserList.FindIndex(b => b.Name == BrowserName);
        //    BrowserList[i] = _browser;
        //    //DEBUG_BOX.Text =  _browser.ExePath;
        //    BrowserName = _browser.Name;

        //    settings.Update();
        }

        private void RemoveBtn(object sender, RoutedEventArgs e)
        {
            //SettingsFile settings = SettingsFile.LoadNewInstance();
            //var BrowserList = settings.SettingsObject.Browsers;

            //_ = BrowserList.Remove(BrowserList.Find(b => b.Name == BrowserName));
            //((StackPanel)Parent).Children.Remove(this);

            //settings.Update();
        }
    }
}
