using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hurl.Selector.Helpers;
using Hurl.Library.Models;
using Hurl.Selector.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WinRT;

namespace Hurl.Selector.ViewModels;

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

    private async void LoadBrowsers(Settings settings)
    {
        Browsers.Clear();

        foreach (var browser in settings.Browsers.Where(b => !b.Hidden))
        {
            BrowserItemViewModel item = new(browser)
            {
                Icon = await _iconLoader.LoadIconAsync(browser)
            };
            Browsers.Add(item);
        }
    }

    private IRelayCommand<BrowserItemViewModel>? launchBrowserCommand;

    public IRelayCommand<BrowserItemViewModel> LaunchBrowserCommand
    {
        get
        {
            return launchBrowserCommand ??= new RelayCommand<BrowserItemViewModel>(LaunchBrowser);
        }
    }

    private void LaunchBrowser(BrowserItemViewModel? browserItem)
    {
        if (browserItem is null)
        {
            return;
        }

        Browser browser = browserItem.Model;
        Debug.WriteLine($"Launching {browser.Name} with URL: {Url}");
        try
        {
            UriLauncher.ResolveAutomatically(Url, browser, null);
            BrowserLaunched?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
