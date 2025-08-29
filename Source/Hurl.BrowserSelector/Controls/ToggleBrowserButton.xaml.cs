using Hurl.Library.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.BrowserSelector.Controls
{
    public partial class ToggleBrowserButton : UserControl
    {
        public ToggleBrowserButton()
        {
            InitializeComponent();
            // Example: Browsers should be set externally, e.g. via binding or code
        }

        public static readonly DependencyProperty SelectedBrowserProperty =
            DependencyProperty.Register(
                nameof(SelectedBrowser),
                typeof(Browser),
                typeof(ToggleBrowserButton),
                new PropertyMetadata(null));

        public Browser SelectedBrowser
        {
            get => (Browser)GetValue(SelectedBrowserProperty);
            set => SetValue(SelectedBrowserProperty, value);
        }

        public static readonly RoutedEvent BrowserToggledEvent =
            EventManager.RegisterRoutedEvent(
                nameof(BrowserToggled),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(ToggleBrowserButton));

        public event RoutedEventHandler BrowserToggled
        {
            add => AddHandler(BrowserToggledEvent, value);
            remove => RemoveHandler(BrowserToggledEvent, value);
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (Resources["BrowserMenu"] is ContextMenu menu)
            {
                menu.PlacementTarget = sender as Button;
                menu.IsOpen = true;
            }
        }

        private void BrowserMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (((MenuItem)sender).Tag is Browser browser)
            {
                SelectedBrowser = browser;
                RaiseEvent(new RoutedEventArgs(BrowserToggledEvent));
            }
        }
    }
}
