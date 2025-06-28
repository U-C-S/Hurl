using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Services;
using Hurl.Library.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.BrowserSelector.Controls
{
    /// <summary>
    /// Interaction logic for BrowserButton.xaml
    /// </summary>
    public partial class BrowserButton : UserControl
    {
        public BrowserButton()
        {
            InitializeComponent();

            if (App.AppHost is IHost AppHost)
            {
                _urlService = AppHost.Services.GetRequiredService<CurrentUrlService>();
            }
            else
            {
                throw new System.Exception("URL Service is not correctly initiated.");
            }
        }

        private readonly CurrentUrlService _urlService;

        public static readonly DependencyProperty BrowserProperty =
            DependencyProperty.Register(
                nameof(Source),
                typeof(Browser),
                typeof(BrowserButton));

        public Browser Source
        {
            get => (Browser)GetValue(BrowserProperty);
            set => SetValue(BrowserProperty, value);
        }

        private void BrowserButton_Click(object sender, RoutedEventArgs e)
        {
            UriLauncher.ResolveAutomatically(_urlService.Get(), Source, null);
            MinimizeWindow();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (((MenuItem)sender).Tag is AlternateLaunch alt)
            {
                UriLauncher.Alternative(_urlService.Get(), Source, alt);
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
