using Hurl.App.Services.Interfaces;
using Hurl.Library;
using Hurl.Library.Models;
using Hurl.Library.Serialization;
using Hurl.Library.Storage;
using System;
using System.Collections.ObjectModel;

namespace Hurl.App.Services;

public class SettingsService : ISettingsService
{
    private readonly JsonFileStore<Settings> store;
    private Settings? settings;

    public SettingsService(string? settingsPath = null)
    {
        store = new JsonFileStore<Settings>(
            settingsPath ?? Constants.APP_SETTINGS_MAIN,
            SelectorJsonSerializerContext.Default.Settings);
    }

    public event EventHandler? SettingsChanged;

    // All windows share this instance; saving one section preserves the others.
    public Settings LoadSettings()
    {
        if (settings is not null)
        {
            return settings;
        }

        bool firstRun = !store.Exists;
        settings = firstRun
            ? new Settings { Browsers = new(GetBrowsers.FromRegistry()) }
            : store.Read()
                ?? new Settings();

        settings.Browsers ??= [];
        settings.AppSettings ??= new();
        settings.QuickView ??= new();
        settings.Rulesets ??= [];
        foreach (var browser in settings.Browsers)
        {
            browser.AlternateLaunches ??= [];
        }

        if (firstRun)
        {
            SaveSettings();
        }

        return settings;
    }

    private void SaveSettings()
    {
        store.Write(settings!);
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateAppSettings(AppSettings appSettings)
    {
        LoadSettings().AppSettings = appSettings;
        SaveSettings();
    }

    public void UpdateQuickView(QuickViewSettings quickView)
    {
        LoadSettings().QuickView = quickView;
        SaveSettings();
    }

    public void UpdateBrowsers(ObservableCollection<Browser> browsers)
    {
        LoadSettings().Browsers = browsers;
        SaveSettings();
    }

    public void UpdateRulesets(ObservableCollection<Ruleset> rulesets)
    {
        LoadSettings().Rulesets = [.. rulesets];
        SaveSettings();
    }
}
