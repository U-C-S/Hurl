using Hurl.Library;
using Hurl.Library.Models;
using Hurl.Selector.Serialization;
using Hurl.Selector.Services.Interfaces;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hurl.Selector.Services;

public class JsonFileService : ISettingsService
{
    private readonly string _settingsPath;

    public JsonFileService(string settingsPath = "settings.json")
    {
        _settingsPath = Constants.APP_SETTINGS_MAIN;
    }

    public async Task<Settings> LoadSettingsAsync()
    {
        if (!File.Exists(_settingsPath))
            return new Settings();

        var json = await File.ReadAllTextAsync(_settingsPath);
        var settings = JsonSerializer.Deserialize(json, SelectorJsonSerializerContext.Default.Settings);
        settings ??= new Settings();

        // Ensure collections are initialized
        settings.Browsers ??= new ObservableCollection<Browser>();
        foreach (var browser in settings.Browsers)
        {
            browser.AlternateLaunches ??= new ObservableCollection<AlternateLaunch>();
        }

        return settings;
    }

    public Settings LoadSettings()
    {
        if (!File.Exists(_settingsPath))
            return new Settings();

        var json = File.ReadAllText(_settingsPath);
        var settings = JsonSerializer.Deserialize(json, SelectorJsonSerializerContext.Default.Settings);
        settings ??= new Settings();

        // Ensure collections are initialized
        settings.Browsers ??= new ObservableCollection<Browser>();
        foreach (var browser in settings.Browsers)
        {
            browser.AlternateLaunches ??= new ObservableCollection<AlternateLaunch>();
        }

        return settings;
    }

    public async Task SaveSettingsAsync(Settings settings)
    {
        var json = JsonSerializer.Serialize(settings, SelectorJsonSerializerContext.Default.Settings);
        await File.WriteAllTextAsync(_settingsPath, json);
    }
}
