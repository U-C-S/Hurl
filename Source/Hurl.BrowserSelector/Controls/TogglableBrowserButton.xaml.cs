using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.Library.Models;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.BrowserSelector.Controls
{
    /// <summary>
    /// Interaction logic for BrowserButton.xaml
    /// </summary>
    public partial class TogglableBrowserButton : UserControl
    {
        public TogglableBrowserButton(Browser browser)
        {
            InitializeComponent();
            DataContext = browser;
        }

        //private void BrowserButton_Click(object sender, RoutedEventArgs e)
        //{
        //    UriLauncher.Default(UriGlobal.Value, (Browser)DataContext);
        //    MinimizeWindow();
        //}

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var alt = ((MenuItem)sender).Tag as AlternateLaunch;
            UriLauncher.Alternative(UriGlobal.Value, (Browser)DataContext, alt);
            MinimizeWindow();
        }

        private void AdditionalBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var x = Resources["TheCM"] as ContextMenu;
            x.PlacementTarget = btn;
            x.IsOpen = true;
            e.Handled = true;
        }

        private void MinimizeWindow()
        {
            var parent = Window.GetWindow(this);
            parent.WindowState = WindowState.Minimized;
            parent.Hide();
        }
    }
}
