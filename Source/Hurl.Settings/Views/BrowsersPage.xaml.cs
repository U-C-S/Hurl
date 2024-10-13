using Hurl.Library;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace Hurl.Settings.Views;

public sealed partial class BrowsersPage : Page
{
    public BrowsersPage()
    {
        this.InitializeComponent();
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
