using Hurl.Library;
using Hurl.Library.Models;
using Hurl.Settings.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hurl.Settings.Services;

public class JsonFileService(IOptions<Library.Models.Settings> settings) : ISettingsService
{
    private readonly string _settingsPath = Constants.APP_SETTINGS_MAIN;

    private IOptions<Library.Models.Settings> settings = settings;

    public async Task SaveSettingsAsync(Library.Models.Settings settings)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(settings, options);
        await File.WriteAllTextAsync(_settingsPath, json);
    }

    public void UpdateAppSettings(AppSettings appSettings)
    {
        var settings = this.settings.Value;
        settings.AppSettings = appSettings;
        SaveSettingsAsync(settings);
    }

    public void UpdateBrowsers(ObservableCollection<Browser> browsers)
    {
        var settings = this.settings.Value;
        settings.Browsers = browsers;
        SaveSettingsAsync(settings);
    }

    public void UpdateRulesets(ObservableCollection<Ruleset> rulesets)
    {
        var settings = this.settings.Value;
        settings.Rulesets = [.. rulesets];
        SaveSettingsAsync(settings);
    }
}

