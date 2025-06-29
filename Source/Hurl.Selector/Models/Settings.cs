using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Hurl.Selector.Models;

public partial class Settings : ObservableObject
{
    [JsonPropertyName("$schema")]
    public string Schema { get; } = "https://raw.githubusercontent.com/U-C-S/Hurl/main/Utils/UserSettings.schema.json";

    //public string Version = Constants.VERSION;

    public string lastUpdated = DateTime.Now.ToString();

    [ObservableProperty]
    public ObservableCollection<Browser> browsers = [];

    [ObservableProperty]
    public AppSettings appSettings = new();

    //public List<Ruleset> Rulesets { get; set; } = [];

    //public static Settings GetSettings()
    //{
    //    try
    //    {
    //        return JsonOperations.FromJsonToModel<Settings>(Constants.APP_SETTINGS_MAIN);
    //    }
    //    catch (Exception e)
    //    {
    //        switch (e)
    //        {
    //            case FileNotFoundException _:
    //            case DirectoryNotFoundException _:
    //                return New(GetBrowsers.FromRegistry());
    //            default:
    //                throw;
    //        }
    //    }
    //}

    //private static Settings New(List<Browser> browsers)
    //{
    //    Directory.CreateDirectory(Constants.APP_SETTINGS_DIR);

    //    var _settings = new Settings()
    //    {
    //        browsers = browsers,
    //    };

    //    JsonOperations.FromModelToJson(_settings, Constants.APP_SETTINGS_MAIN);
    //    return _settings;
    //}
}
