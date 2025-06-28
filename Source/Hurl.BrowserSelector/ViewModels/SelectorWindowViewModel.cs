using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.BrowserSelector.Services;
using Hurl.Library.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hurl.BrowserSelector.ViewModels;

internal partial class SelectorWindowViewModel : ObservableObject
{
    private readonly CurrentUrlService _urlService;

    [ObservableProperty]
    private string url;

    [ObservableProperty]
    private string urlUiString;

    [ObservableProperty]
    public ObservableCollection<Browser> browsers;

    [ObservableProperty]
    public List<Ruleset> rulesets;

    [ObservableProperty]
    public AppSettings otherSettings;

    // Used by URL Editor
    partial void OnUrlChanged(string value)
    {
        _urlService.Set(value);
        UrlUiString = string.IsNullOrEmpty(Url) ? "No Url Opened" : Url;
    }


    public SelectorWindowViewModel(IOptionsMonitor<Settings> settings, CurrentUrlService urlService)
    {
        _urlService = urlService;
        _urlService.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(CurrentUrlService.Url))
            {
                var newUrl = _urlService.Get();
                if (Url == newUrl)
                    return;
                else
                {
                    Url = newUrl;
                }
            }
        };

        Url = _urlService.Url;

        this.Browsers = settings.CurrentValue.Browsers;
        this.Rulesets = settings.CurrentValue.Rulesets;
        this.otherSettings = settings.CurrentValue.AppSettings;

        settings.OnChange((s, _) =>
        {
            Browsers.Clear();
            foreach (var browser in s.Browsers)
            {
                Browsers.Add(browser);
            }
        });
    }
}

