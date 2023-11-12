using Hurl.Library;
using Hurl.Library.Models;
using System.Collections.Generic;

namespace Hurl.SettingsApp.State
{
    public class Settings
    {
        private static Settings _instance = new();

        private Library.Models.Settings _settings = SettingsFile.GetSettings();

        private static Library.Models.Settings Value
        {
            get
            {
                return _instance._settings;
            }
            set
            {
                _instance._settings = value;
            }
        }

        public static void AddBrowserRule(List<string> rules, string name)
        {
            Value.AutoRoutingRules.Add(new AutoRoutingRules() { Rules = rules, BrowserName = name });

            Save();
        }

        public static List<Browser> GetBrowsers()
        {
            return Library.GetBrowsers.FromSettingsFile(Value, false);
        }

        public static List<AutoRoutingRules> GetAutoRoutingRules()
        {
            return Value.AutoRoutingRules;
        }

        #region AppSettingsMethods

        public static AppSettings GetAppSettings()
        {
            return Value.AppSettings;
        }

        public static void Set_LaunchUnderMouse(bool value)
        {
            Value.AppSettings.LaunchUnderMouse = value;
            Save();
        }

        public static void Set_NoWhiteBorder(bool value)
        {
            Value.AppSettings.NoWhiteBorder = value;
            Save();
        }

        public static void Set_BackgroundType(string value)
        {
            Value.AppSettings.BackgroundType = value;
            Save();
        }
        #endregion

        private static void Save()
        {
            JsonOperations.FromModelToJson(_instance._settings, Constants.APP_SETTINGS_MAIN);
        }
    }
}
