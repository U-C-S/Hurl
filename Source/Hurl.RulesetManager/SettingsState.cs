using Hurl.Library;
using Hurl.Library.Models;

namespace Hurl.RulesetManager;

internal class SettingsState
{
    private SettingsState()
    {
        _settings = SettingsFile.GetSettings();
    }

    private static readonly SettingsState _state = new();

    private Settings _settings;

    public static Settings Get => _state._settings;

    public static List<Ruleset> Rulesets
    {
        get
        {
            if (Get.Rulesets == null)
            {
                Get.Rulesets = new();
            }
            return Get.Rulesets;
        }
        set
        {
            _state._settings.Rulesets = value;
        }
    }

    public static List<Browser> GetBrowsers() => Get.Browsers;

    public static void Update()
    {
        _state._settings.LastUpdated = DateTime.Now.ToString();

        JsonOperations.FromModelToJson(_state._settings, Constants.APP_SETTINGS_MAIN);
    }
}

