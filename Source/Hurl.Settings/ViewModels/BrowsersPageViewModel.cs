using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.Settings.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hurl.Settings.ViewModels;

internal partial class BrowsersPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial ObservableCollection<Browser> Browsers { get; set; }

    private ISettingsService settingsService;

    public BrowsersPageViewModel(IOptionsMonitor<Library.Models.Settings> settings, ISettingsService settingsService)
    {
        this.settingsService = settingsService;
        Browsers = settings.CurrentValue.Browsers;

        //settings.OnChange((s, _) =>
        //{
        //    Browsers.Clear();
        //    foreach (var browser in s.Browsers)
        //    {
        //        Browsers.Add(browser);
        //    }
        //});
    }

    public void RefreshBrowserList(BrowserRefreshMode mode)
    {
        foreach (var browser in Library.GetBrowsers.FromRegistry())
        {
            if (mode == BrowserRefreshMode.AddAllDetectedAsNew
                || !Browsers.Any(existing => existing.ExePath == browser.ExePath))
            {
                Browsers.Add(browser);
            }
        }
        settingsService.UpdateBrowsers(Browsers);
    }
    public void DeleteBrowser(Guid browserId)
    {
        var browser = Browsers.FirstOrDefault(browser => browser.Id == browserId);
        if (browser != null && Browsers.Remove(browser))
        {
            settingsService.UpdateBrowsers(Browsers);
        }
    }

    internal void UpdateBrowserOrder()
    {
        settingsService.UpdateBrowsers(Browsers);
    }
}

internal enum BrowserRefreshMode
{
    PreserveExistingByExePath,
    AddAllDetectedAsNew
}
