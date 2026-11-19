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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSelectDuration))]
    public partial bool HasTransientDefault { get; set; }

    [ObservableProperty]
    public partial string DurationButtonText { get; set; } = "Select a browser";

    [ObservableProperty]
    public partial string DurationButtonDescription { get; set; } = "Choose how long to use a browser";

    [ObservableProperty]
    public partial string RemainingTimeText { get; set; } = string.Empty;

    public bool CanSelectDuration => !HasTransientDefault;

    private string BrowserSelectionText => TransientBrowserDuration > 0
        ? $"Select a browser · {TransientBrowserDuration} min"
        : "Select a browser";

    partial void OnTransientBrowserDurationChanged(int value)
    {
        OnPropertyChanged(nameof(IsTransientDurationSelected));
        if (!HasTransientDefault)
        {
            DurationButtonText = BrowserSelectionText;
        }
    }

    partial void OnHasTransientDefaultChanged(bool value)
    {
        transientDurationCommand?.NotifyCanExecuteChanged();
        addFifteenMinutesCommand?.NotifyCanExecuteChanged();
        endTransientDefaultCommand?.NotifyCanExecuteChanged();
    }

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
        RefreshTransientDefault();
        foreach (var item in items)
        {
            item.Icon = await _iconLoader.LoadIconAsync(item.Model);
        }
    }

    public void RefreshTransientDefault()
    {
        try
        {
            TransientBrowser? activeBrowser = transientDefaultBrowserService.GetActiveBrowser();
            TimeSpan remaining = transientDefaultBrowserService.GetRemainingTime();
            if (remaining <= TimeSpan.Zero)
            {
                activeBrowser = null;
            }

            HasTransientDefault = activeBrowser is not null;
            if (HasTransientDefault)
            {
                TransientBrowserDuration = 0;
            }

            RemainingTimeText = HasTransientDefault
                ? $"{Math.Ceiling(remaining.TotalMinutes)} min left"
                : string.Empty;
            DurationButtonText = activeBrowser is null
                ? BrowserSelectionText
                : $"{activeBrowser.Name} · {RemainingTimeText}";
            DurationButtonDescription = activeBrowser is null
                ? "Choose how long to use a browser"
                : $"Links open in {activeBrowser.Name} until the timer ends.";

            bool availabilityChanged = false;
            foreach (var item in Browsers)
            {
                bool enabled = activeBrowser is null || item.Model.Id == activeBrowser.BrowserId;
                availabilityChanged |= item.IsEnabled != enabled;
                item.IsEnabled = enabled;
                item.AllowAlternateLaunches = activeBrowser is null;
            }

            if (availabilityChanged)
            {
                launchBrowserCommand?.NotifyCanExecuteChanged();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private IRelayCommand<BrowserItemViewModel>? launchBrowserCommand;

    public IRelayCommand<BrowserItemViewModel> LaunchBrowserCommand =>
        launchBrowserCommand ??= new RelayCommand<BrowserItemViewModel>(
            LaunchBrowser, item => item?.IsEnabled == true);

    private void LaunchBrowser(BrowserItemViewModel? browserItem)
    {
        if (browserItem is not null)
        {
            LaunchBrowser(browserItem.Model);
        }
    }

    private IRelayCommand<string>? transientDurationCommand;

    public IRelayCommand<string> TransientDurationCommand =>
        transientDurationCommand ??= new RelayCommand<string>(SetTransientDuration, _ => CanSelectDuration);

    public bool IsTransientDurationSelected(int optionMinutes) =>
        TransientBrowserDuration == optionMinutes;

    private void SetTransientDuration(string? value)
    {
        RefreshTransientDefault();
        if (CanSelectDuration && int.TryParse(value, out int minutes) && minutes is 0 or 15 or 30 or 60 or 120)
        {
            TransientBrowserDuration = minutes;
        }
    }

    private IRelayCommand? addFifteenMinutesCommand;

    public IRelayCommand AddFifteenMinutesCommand =>
        addFifteenMinutesCommand ??= new RelayCommand(AddFifteenMinutes, () => HasTransientDefault);

    private void AddFifteenMinutes()
    {
        try
        {
            transientDefaultBrowserService.AddFifteenMinutes();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        RefreshTransientDefault();
    }

    private IRelayCommand? endTransientDefaultCommand;

    public IRelayCommand EndTransientDefaultCommand =>
        endTransientDefaultCommand ??= new RelayCommand(EndTransientDefault, () => HasTransientDefault);

    private void EndTransientDefault()
    {
        try
        {
            transientDefaultBrowserService.End();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        RefreshTransientDefault();
    }

    public void LaunchBrowser(Browser browser, Guid? alternateLaunchId = null)
    {
        Debug.WriteLine($"Launching {browser.Name} with URL: {Url}");
        try
        {
            TransientBrowser? activeBrowser = transientDefaultBrowserService.GetActiveBrowser();
            if (activeBrowser is not null)
            {
                if (browser.Id != activeBrowser.BrowserId
                    || (alternateLaunchId is not null && alternateLaunchId != activeBrowser.AltLaunchId))
                {
                    return;
                }

                // The enabled tile always uses the saved profile and arguments.
                UriLauncher.Default(Url, activeBrowser);
            }
            else
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
            }

            BrowserLaunched?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            RefreshTransientDefault();
        }
    }
}
