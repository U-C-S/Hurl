using Hurl.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Hurl.App.Views;

public sealed partial class QuickViewPage : Page
{
    public QuickViewPage()
    {
        InitializeComponent();

        ViewModel = App.Services!.GetRequiredService<QuickViewPageViewModel>();
    }

    public QuickViewPageViewModel ViewModel { get; }
}
