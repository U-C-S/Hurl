using Hurl.Library;
using Hurl.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;

namespace Hurl.App.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        this.InitializeComponent();
        NavigationCacheMode = NavigationCacheMode.Required;
        ViewModel = App.Services!.GetRequiredService<SettingsPageViewModel>();
    }
    public SettingsPageViewModel ViewModel { get; }

    private async void DefaultAppButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await global::Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:defaultapps"));
    }

    private void EditJsonButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Process.Start("explorer", "\"" + Constants.APP_SETTINGS_MAIN + "\"");
    }
}
