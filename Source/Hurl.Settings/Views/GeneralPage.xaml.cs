using Hurl.Settings.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Hurl.Settings.Views;

public sealed partial class GeneralPage : Page
{
    public GeneralPage()
    {
        InitializeComponent();
    }

    public SettingsViewModel ViewModel => new();

    private async void DefaultAppButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:defaultapps"));
    }
}
