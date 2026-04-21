using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hurl.Selector.Helpers;
using Hurl.Selector.Models;
using Hurl.Selector.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WinRT;

namespace Hurl.Selector.ViewModels;

[GeneratedBindableCustomPropertyAttribute]
public partial class SelectorPageViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly IIconLoader _iconLoader;

    public SelectorPageViewModel(ISettingsService settingsService, IIconLoader iconLoader)
    {
        _settingsService = settingsService;
        _iconLoader = iconLoader;
        LoadBrowsers();
    }

    [ObservableProperty]
    public partial string Url { get; set; } = string.Empty;

    [ObservableProperty]
    public partial ObservableCollection<Browser> Browsers { get; set; } = new();

    private async void LoadBrowsers()
    {
        var settings = await _settingsService.LoadSettingsAsync();
        Browsers.Clear();

        foreach (var browser in settings.Browsers.Where(b => !b.Hidden))
        {
            browser.Icon = await _iconLoader.LoadIconFromExe(browser.ExePath);
            Browsers.Add(browser);
        }
    }

    private IRelayCommand<Browser>? launchBrowserCommand;

    public IRelayCommand<Browser> LaunchBrowserCommand
    {
        get
        {
            return launchBrowserCommand ??= new RelayCommand<Browser>(LaunchBrowser);
        }
    }

    private void LaunchBrowser(Browser? browser)
    {
        if (browser is null)
        {
            return;
        }

        Debug.WriteLine($"Launching {browser.Name} with URL: {Url}");
        try
        {
            UriLauncher.Default(Url, browser);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
