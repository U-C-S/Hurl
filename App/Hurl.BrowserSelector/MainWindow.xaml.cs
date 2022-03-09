using Hurl.BrowserSelector.Controls;
using Hurl.SharedLibraries.Models;
using Hurl.SharedLibraries.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;
using System.Windows.Input;
using System.Diagnostics;
using Hurl.SharedLibraries.Constants;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string OpenedLink;

        public MainWindow(string URL)
        {
            WPFUI.Background.Manager.Apply(WPFUI.Background.BackgroundType.Acrylic, this);

            InitializeComponent();
            WindowChrome.SetWindowChrome(this, new WindowChrome()
            {
                CaptionHeight = 5
            });

            linkpreview.Text = OpenedLink = URL;

            Window_Loaded();
        }

        private void Window_Loaded()
        {
            Stopwatch sw = new();
            sw.Start();
            //var y = from z in x where z.ExePath is not null and z.Hidden is false select z;
            IEnumerable<Browser> LoadableBrowsers = from b in SettingsFile.LoadNewInstance().SettingsObject.Browsers
                                                    where b.Name != null && b.ExePath != null && b.Hidden != true
                                                    select b;
            sw.Stop();
            Debug.WriteLine("---------" + sw.ElapsedMilliseconds.ToString());

            foreach (Browser i in LoadableBrowsers)
            {
                BrowserIconBtn browserUC = new BrowserIconBtn(i, OpenedLink);

                var separator = new System.Windows.Shapes.Rectangle()
                {
                    Width = 2,
                    Height = 40,
                    Margin = new Thickness(3, 0, 3, 20),
                    Fill = System.Windows.Media.Brushes.AliceBlue
                };

                _ = stacky.Children.Add(browserUC);
                //_ = stacky.Children.Add(separator);
            }

            //stacky.Children.RemoveAt(stacky.Children.Count - 1);
        }

        private void Window_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void LinkCopyBtnClick(object sender, RoutedEventArgs e) => Clipboard.SetText(OpenedLink);
        private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start("notepad.exe", MetaStrings.SettingsFilePath);
        private void CloseBtnClick(object sender, RoutedEventArgs e) => this.Close();


        //private void draggable(object sender, MouseButtonEventArgs e) => this.DragMove();
    }
}
