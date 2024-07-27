using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.Library.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Hurl.BrowserSelector.Controls
{
    /// <summary>
    /// Interaction logic for BrowserButton.xaml
    /// </summary>
    public partial class TogglableBrowserButton : UserControl
    {
        public int BrowserIndex { get; }

        public Browser Browser { get; }

        public bool? IsChecked
        {
            get => ToggleBtn.IsChecked;
            set
            {
                ToggleBtn.IsChecked = value;
            }
        }

        private readonly Action<int, bool> _clickCallback;

        public TogglableBrowserButton(Browser browser, int index, Action<int, bool> clickCallback)
        {
            Browser = browser;
            BrowserIndex = index;
            _clickCallback = clickCallback;

            InitializeComponent();
            DataContext = this;
        }

        private void BrowserButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(IsChecked);
            _clickCallback(BrowserIndex, IsChecked ?? false);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var alt = ((MenuItem)sender).Tag as AlternateLaunch;
            //UriLauncher.Alternative(UriGlobal.Value, (Browser)DataContext, alt);
            _clickCallback(BrowserIndex, IsChecked ?? false);
        }

        private void AdditionalBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var x = Resources["TheCM"] as ContextMenu;
            x.PlacementTarget = btn;
            x.IsOpen = true;
            e.Handled = true;
        }
    }
}
