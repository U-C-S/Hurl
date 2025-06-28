using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.Settings.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Hurl.Settings.ViewModels;

internal partial class BrowsersPageViewModel: ObservableObject
{
    [ObservableProperty]
    public ObservableCollection<Browser> browsers;

    private ISettingsService settingsService;

    public BrowsersPageViewModel(IOptionsMonitor<Library.Models.Settings> settings, ISettingsService settingsService)
    {
        this.settingsService = settingsService;
        browsers = settings.CurrentValue.Browsers;

        settings.OnChange((s, _) =>
        {
            Browsers.Clear();
            foreach (var browser in s.Browsers)
            {
                Browsers.Add(browser);
            }
        });
    }

    public void RefreshBrowserList() => RefreshBrowsers();

    public void RefreshBrowsers()
    {
        var refreshedBrowsers = Library.GetBrowsers.FromRegistry();
        List<Browser> newList = [.. Browsers];

        // Go over the new browser list and add any of those browsers that are not already present
        // in the existing browser list
        foreach (var newBrowser in refreshedBrowsers)
        {
            var isExists = newList.Any(b => b.ExePath == newBrowser.ExePath);
            if (!isExists)
            {
                Browsers.Add(newBrowser);
            }
        }

        settingsService.UpdateBrowsers(Browsers);
    }
}
