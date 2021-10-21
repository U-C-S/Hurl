using Hurl.BrowserSelector.Controls;
using Hurl.SharedLibraries.Models;
using Hurl.SharedLibraries.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string OpenedLink = null;

        public MainWindow(string URL)
        {
            InitializeComponent();

            linkpreview.Text = OpenedLink = URL;

            Window_Loaded();

            //What does \"%1\" mean in Registry ? 
            //https://www.tek-tips.com/viewthread.cfm?qid=382878
        }

        private void Window_Loaded()
        {
            //var y = from z in x where z.ExePath is not null and z.Hidden is false select z;
            IEnumerable<Browser> LoadableBrowsers = from b in SettingsFile.LoadNewInstance().SettingsObject.Browsers
                                                    where b.Name != null && b.ExePath != null && b.Hidden != true
                                                    select b;

            foreach (Browser i in LoadableBrowsers)
            {
                BrowserIconBtn browserUC = new BrowserIconBtn()
                {
                    BrowserName = i.Name,
                    BrowserIcon = i.GetIcon,
                    ExePath = i.ExePath,
                    URL = OpenedLink,
                    LaunchArgs = i.LaunchArgs,
                    AlternateLaunches = i.AlternateLaunches
                };

                var separator = new System.Windows.Shapes.Rectangle()
                {
                    Width = 1,
                    Height = 40,
                    Margin = new Thickness(3, 0, 3, 20),
                    Fill = System.Windows.Media.Brushes.AliceBlue
                };

                _ = stacky.Children.Add(browserUC);
                _ = stacky.Children.Add(separator);
            }

            stacky.Children.RemoveAt(stacky.Children.Count - 1);
        }

        private void Window_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Window_Deactivated(object sender, EventArgs e) => Close();

        private void LinkCopyBtn(object sender, RoutedEventArgs e) => Clipboard.SetText(OpenedLink);
    }
}
