using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.BrowserSelector.Services;
using Hurl.Library.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hurl.BrowserSelector.ViewModels;

internal partial class SelectorWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string url;

    [ObservableProperty]
    public ObservableCollection<Browser> browsers;

    [ObservableProperty]
    public List<Ruleset> rulesets;

    [ObservableProperty]
    public AppSettings otherSettings;

    public SelectorWindowViewModel(IOptionsMonitor<Settings> settings, CurrentUrlService urlService)
    {
        Url = urlService.Url;

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

