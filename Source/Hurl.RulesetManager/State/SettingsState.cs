using Hurl.Library;
using Hurl.Library.Models;

namespace Hurl.RulesetManager.State;

class SettingsState
{
    private Settings _settings = SettingsFile.GetSettings();

    private static SettingsState _instance = new();

    public static Settings GetCurrent
    {
        get
        {
            return _instance._settings;
        }
    }

    public static void Save(Settings settings)
    {
        settings.LastUpdated = DateTime.Now.ToString();

        _instance._settings = settings;
        JsonOperations.FromModelToJson(settings, Constants.APP_SETTINGS_MAIN);
    }

}
