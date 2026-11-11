using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hurl.App.Helpers;
using Hurl.App.Services.Interfaces;
using Hurl.Library.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WinRT;

namespace Hurl.App.ViewModels;

[GeneratedBindableCustomProperty]
public partial class SelectorPageViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly IIconLoader _iconLoader;
    public event EventHandler? BrowserLaunched;

    public SelectorPageViewModel(ISettingsService settingsService, IIconLoader iconLoader)
    {
        _settingsService = settingsService;
        _iconLoader = iconLoader;
        Settings settings = _settingsService.LoadSettings();
        AppSettings = settings.AppSettings ?? new AppSettings();
        LoadBrowsers(settings);
    }

    [ObservableProperty]
    public partial string Url { get; set; } = string.Empty;

    [ObservableProperty]
    public partial ObservableCollection<BrowserItemViewModel> Browsers { get; set; } = new();

    [ObservableProperty]
    public partial AppSettings AppSettings { get; set; }

    public void RefreshSettings()
    {
        Settings settings = _settingsService.LoadSettings();
        AppSettings = settings.AppSettings;
        LoadBrowsers(settings);
    }

    private async void LoadBrowsers(Settings settings)
    {
        // Snapshot before awaiting so editing/reordering browsers cannot invalidate enumeration.
        var items = settings.Browsers.Where(browser => !browser.Hidden)
            .Select(browser => new BrowserItemViewModel(browser)).ToArray();
        Browsers = new(items);
        foreach (var item in items)
        {
            item.Icon = await _iconLoader.LoadIconAsync(item.Model);
        }
    }

    private IAsyncRelayCommand<BrowserItemViewModel>? launchBrowserCommand;

    public IAsyncRelayCommand<BrowserItemViewModel> LaunchBrowserCommand
    {
        get
        {
            return launchBrowserCommand ??= new AsyncRelayCommand<BrowserItemViewModel>(LaunchBrowserAsync);
        }
    }

    private async Task LaunchBrowserAsync(BrowserItemViewModel? browserItem)
    {
        if (browserItem is null)
        {
            return;
        }

        Browser browser = browserItem.Model;
        Debug.WriteLine($"Launching {browser.Name} with URL: {Url}");
        try
        {
            await UriLauncher.ResolveAutomaticallyAsync(Url, browser, null);
            BrowserLaunched?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
