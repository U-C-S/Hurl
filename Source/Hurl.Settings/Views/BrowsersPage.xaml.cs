using Hurl.Library;
using Hurl.Library.Models;
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

    private void EditBrowser_Click(SplitButton sender, SplitButtonClickEventArgs e)
    {
        if (sender is SplitButton btn && btn.DataContext is Browser browser)
        {
            Frame?.Navigate(typeof(Hurl.Settings.Views.Dialogs.EditBrowserPage), browser);
        }
    }

    private void ListView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
    {
        ViewModel.UpdateBrowserOrder();
    }
}
