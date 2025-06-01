using Hurl.BrowserSelector.Helpers;
using Hurl.Library.Models;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.BrowserSelector.Controls
{
    /// <summary>
    /// Interaction logic for BrowserButton.xaml
    /// </summary>
    public partial class BrowserButton : UserControl
    {
        public BrowserButton(Browser browser)
        {
            InitializeComponent();
            DataContext = browser;
        }

        private void BrowserButton_Click(object sender, RoutedEventArgs e)
        {
            //UriLauncher.ResolveAutomatically(OpenedUri.Value, (Browser)DataContext, null);
            MinimizeWindow();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (((MenuItem)sender).Tag is AlternateLaunch alt)
            {
                //UriLauncher.Alternative(OpenedUri.Value, (Browser)DataContext, alt);
                MinimizeWindow();
            }
        }

        private void AdditionalBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (Resources["TheCM"] is ContextMenu x)
            {
                x.PlacementTarget = btn;
                x.IsOpen = true;
            }
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
