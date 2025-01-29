using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hurl.BrowserSelector.Helpers;
using Hurl.Selector.Models;
using Hurl.Selector.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hurl.Selector.ViewModels;
public partial class SelectorPageViewModel: ObservableObject
{
    private readonly ISettingsService _settingsService;

    public SelectorPageViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
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
            //var icon = await _iconLoader.LoadIconAsync(browser.ExePath);
            Browsers.Add(browser);
        }
    }

    [RelayCommand]
    private void LaunchBrowser(Browser browser)
    {
        try
        {
            UriLauncher.Default(url, browser);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
