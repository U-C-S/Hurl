using Hurl.App.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Hurl.App.ViewModels;

internal class BrowsersPageViewModel
{
    private readonly ISettingsService settingsService;
    private readonly IIconLoader iconLoader;

    public ObservableCollection<BrowserItemViewModel> Browsers { get; }

    public BrowsersPageViewModel(ISettingsService settingsService, IIconLoader iconLoader)
    {
        this.settingsService = settingsService;
        this.iconLoader = iconLoader;
        Browsers = new(settingsService.LoadSettings().Browsers.Select(browser => new BrowserItemViewModel(browser)));
    }

    public async Task LoadIconsAsync()
    {
        foreach (var item in Browsers.ToArray())
        {
            item.Icon = await iconLoader.LoadIconAsync(item.Model);
        }
    }

    public async Task RefreshBrowserListAsync(BrowserRefreshMode mode)
    {
        foreach (var browser in Library.GetBrowsers.FromRegistry())
        {
            if (mode == BrowserRefreshMode.AddAllDetectedAsNew
                || !Browsers.Any(existing => existing.Model.ExePath == browser.ExePath))
            {
                Browsers.Add(new BrowserItemViewModel(browser));
            }
        }
        SaveBrowsers();
        await LoadIconsAsync();
    }

    public void DeleteBrowser(Guid browserId)
    {
        var browser = Browsers.FirstOrDefault(browser => browser.Model.Id == browserId);
        if (browser != null && Browsers.Remove(browser))
        {
            SaveBrowsers();
        }
    }

    internal void UpdateBrowserOrder() => SaveBrowsers();

    private void SaveBrowsers() => settingsService.UpdateBrowsers(new(Browsers.Select(item => item.Model)));
}

internal enum BrowserRefreshMode
{
    PreserveExistingByExePath,
    AddAllDetectedAsNew
}
