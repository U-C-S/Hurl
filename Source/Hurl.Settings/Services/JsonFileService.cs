using Hurl.Library;
using Hurl.Library.Models;
using Hurl.Library.Serialization;
using Hurl.Settings.Services.Interfaces;
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

    public void SaveSettings(Library.Models.Settings settings)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        var json = JsonSerializer.Serialize(settings, SelectorJsonSerializerContext.Default.Settings);
        File.WriteAllText(_settingsPath, json);
    }

    public void UpdateAppSettings(AppSettings appSettings)
    {
        var settings = this.settings.Value;
        settings.AppSettings = appSettings;
        SaveSettings(settings);
    }

    public void UpdateBrowsers(ObservableCollection<Browser> browsers)
    {
        var settings = this.settings.Value;
        settings.Browsers = browsers;
        SaveSettings(settings);
    }

    public void UpdateRulesets(ObservableCollection<Ruleset> rulesets)
    {
        var settings = this.settings.Value;
        settings.Rulesets = [.. rulesets];
        SaveSettings(settings);
    }
}

