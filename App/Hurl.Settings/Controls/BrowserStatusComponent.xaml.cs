using Hurl.Settings.Views;
using Hurl.SharedLibraries.Services;
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
        public string BackColor { get; set; } = "Black";
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

        private void EditTheBrowser(object sender, RoutedEventArgs e)
        {
            BrowserForm form = new BrowserForm()
            {
                BrowserName = BrowserName,
                BrowserPath = BrowserPath,
            };

            if (form.ShowDialog() == true)
            {
                SettingsFile settings = SettingsFile.LoadNewInstance();
                var BrowserList = settings.SettingsObject.Browsers;

                string name = form.BrowserName;
                string path = form.BrowserPath;

                if (name.Equals("") && path.Equals(""))
                {
                    _ = BrowserList.Remove(BrowserList.Find(b => b.Name == BrowserName));

                    ((StackPanel)Parent).Children.Remove(this);
                }
                else
                {
                    var mod = BrowserList.Find(b => b.Name == BrowserName);
                    mod.Name = BrowserName = BrowserNameTextBlock.Text = name;
                    mod.ExePath = BrowserPath = path;
                }

                settings.Update();
            }
        }
    }
}
