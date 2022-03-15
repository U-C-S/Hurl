using Hurl.BrowserSelector.Controls;
using Hurl.BrowserSelector.Helpers;
using Hurl.SharedLibraries.Constants;
using Hurl.SharedLibraries.Models;
using Hurl.SharedLibraries.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WPFUI.Background;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CurrentLink OpenedLink = new("");

        public MainWindow()
        {
            Manager.Apply(BackgroundType.Acrylic, this);

            InitializeComponent();

            RoundedCorners.Apply(this, () => WindowBorder.CornerRadius = new CornerRadius(0));

            ShowBrowserIcons();
        }

        private void ShowBrowserIcons()
        {
#if DEBUG
            Stopwatch sw = new();
            sw.Start();
#endif
            IEnumerable<Browser> LoadableBrowsers = from b in SettingsFile.LoadNewInstance().SettingsObject.Browsers
                                                    where b.Name != null && b.ExePath != null && b.Hidden != true
                                                    select b;
#if DEBUG
            sw.Stop();
            Debug.WriteLine("---------" + sw.ElapsedMilliseconds.ToString());
#endif

            foreach (Browser i in LoadableBrowsers)
            {
                _ = stacky.Children.Add(new BrowserIconBtn(i, OpenedLink));
            }
        }

        public void Init(string URL)
        {
            if (!IsActive)
            {
                Show();
            }
            OpenedLink.Url = URL;
            linkpreview.Text = URL;
        }

        private void Window_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MinimizeWindow();
            }
        }

        private void LinkCopyBtnClick(object sender, RoutedEventArgs e) => Clipboard.SetText(OpenedLink.Url);
        private void SettingsBtnClick(object sender, RoutedEventArgs e) => Process.Start("notepad.exe", MetaStrings.SettingsFilePath);
        private void Draggable(object sender, MouseButtonEventArgs e) => this.DragMove();
        private void CloseBtnClick(object sender, RoutedEventArgs e) => MinimizeWindow();

        private void MinimizeWindow()
        {
            this.WindowState = WindowState.Minimized;
        }
    }

    public class CurrentLink : INotifyPropertyChanged
    {
        public CurrentLink(string URL)
        {
            this._url = URL;
        }
        private string _url;

        public string Url
        {
            get => _url;
            set
            {
                if (value != _url)
                {
                    _url = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
