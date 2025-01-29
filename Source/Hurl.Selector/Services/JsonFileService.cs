using Hurl.Library;
using Hurl.Selector.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        var settings = JsonSerializer.Deserialize<Settings>(json, options);

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
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(settings, options);
        await File.WriteAllTextAsync(_settingsPath, json);
    }
}

