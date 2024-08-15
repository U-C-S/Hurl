using Hurl.Settings.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Hurl.Settings.Views;

public sealed partial class Settings : Page
{
    public Settings()
    {
        this.InitializeComponent();
        NavigationCacheMode = NavigationCacheMode.Required;
    }
    public SettingsViewModel ViewModel => new();
}
