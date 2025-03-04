using Hurl.Library;
using Hurl.Settings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace Hurl.Settings.Views;

public sealed partial class BrowsersPage : Page
{
    internal BrowsersPageViewModel ViewModel { get; }

    public BrowsersPage()
    {
        this.InitializeComponent();
        ViewModel = App.AppHost.Services.GetRequiredService<BrowsersPageViewModel>();

    }

    private void RefreshButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.RefreshBrowserList();
    }

    private void EditJsonButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Process.Start("explorer", "\"" + Constants.APP_SETTINGS_MAIN + "\"");
    }
}
