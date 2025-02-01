using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hurl.BrowserSelector.Helpers;
using Hurl.Selector.Models;
using Hurl.Selector.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Hurl.Selector.ViewModels;

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
    private string url = "https://google.com";

    [ObservableProperty]
    private ObservableCollection<Browser> browsers = new();

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

    [RelayCommand]
    private void LaunchBrowser(Browser browser)
    {
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
