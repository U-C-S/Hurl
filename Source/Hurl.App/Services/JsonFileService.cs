using Hurl.App.Services.Interfaces;
using Hurl.Library;
using Hurl.Library.Models;
using Hurl.Library.Serialization;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Hurl.App.Services;

public class JsonFileService : ISettingsService
{
    private readonly string settingsPath;
    private Settings? settings;

    public JsonFileService(string? settingsPath = null)
    {
        this.settingsPath = settingsPath ?? Constants.APP_SETTINGS_MAIN;
    }

    public event EventHandler? SettingsChanged;

    // All windows share this instance; saving one section preserves the others.
    public Settings LoadSettings()
    {
        if (settings is not null)
        {
            return settings;
        }

        bool firstRun = !File.Exists(settingsPath);
        settings = firstRun
            ? new Settings { Browsers = new(GetBrowsers.FromRegistry()) }
            : JsonSerializer.Deserialize(File.ReadAllText(settingsPath), SelectorJsonSerializerContext.Default.Settings)
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
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(settingsPath))!);
        string json = JsonSerializer.Serialize(settings, SelectorJsonSerializerContext.Default.Settings);
        // Replace only after the complete document is written.
        string temporaryPath = settingsPath + ".tmp";
        File.WriteAllText(temporaryPath, json);
        File.Move(temporaryPath, settingsPath, overwrite: true);
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
