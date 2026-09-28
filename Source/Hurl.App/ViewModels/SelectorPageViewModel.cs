using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hurl.App.Helpers;
using Hurl.App.Services.Interfaces;
using Hurl.Library.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WinRT;

namespace Hurl.App.ViewModels;

[GeneratedBindableCustomProperty]
public partial class SelectorPageViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly IIconLoader _iconLoader;
    private readonly ITransientDefaultBrowserService transientDefaultBrowserService;
    public event EventHandler? BrowserLaunched;

    public SelectorPageViewModel(
        ISettingsService settingsService,
        IIconLoader iconLoader,
        ITransientDefaultBrowserService transientDefaultBrowserService)
    {
        _settingsService = settingsService;
        _iconLoader = iconLoader;
        this.transientDefaultBrowserService = transientDefaultBrowserService;
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

    [ObservableProperty]
    public partial int TransientBrowserDuration { get; set; }

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

        LaunchBrowser(browserItem.Model);
    }

    private IRelayCommand<string>? transientDurationCommand;

    public IRelayCommand<string> TransientDurationCommand =>
        transientDurationCommand ??= new RelayCommand<string>(SetTransientDuration);

    public bool IsTransientDurationSelected(int optionMinutes) =>
        TransientBrowserDuration == optionMinutes;

    private void SetTransientDuration(string? value)
    {
        if (int.TryParse(value, out int minutes) && minutes is 0 or 15 or 30 or 60 or 120)
        {
            TransientBrowserDuration = minutes;
        }
    }

    public void LaunchBrowser(Browser browser, Guid? alternateLaunchId = null)
    {
        Debug.WriteLine($"Launching {browser.Name} with URL: {Url}");
        try
        {
            UriLauncher.ResolveAutomatically(Url, browser, alternateLaunchId);
            if (TransientBrowserDuration > 0)
            {
                transientDefaultBrowserService.Start(
                    browser,
                    TimeSpan.FromMinutes(TransientBrowserDuration),
                    alternateLaunchId);

                TransientBrowserDuration = 0;
            }
            BrowserLaunched?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
