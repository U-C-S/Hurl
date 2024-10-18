using Hurl.Settings.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace Hurl.Settings.Views;

public sealed partial class GeneralPage : Page
{
    public GeneralPage()
    {
        this.InitializeComponent();
        NavigationCacheMode = NavigationCacheMode.Required;
    }
    public SettingsViewModel ViewModel => new();

    private async void DefaultAppButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:defaultapps"));
    }
}
