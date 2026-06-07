using Hurl.Settings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Hurl.Settings.Views;

public sealed partial class QuickViewPage : Page
{
    public QuickViewPage()
    {
        InitializeComponent();
        NavigationCacheMode = NavigationCacheMode.Required;
        ViewModel = App.AppHost.Services.GetRequiredService<QuickViewPageViewModel>();
    }

    public QuickViewPageViewModel ViewModel { get; }
}
