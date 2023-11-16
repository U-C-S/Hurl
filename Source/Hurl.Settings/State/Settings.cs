using Hurl.Library;
using Hurl.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hurl.Settings.State
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

        #region BrowserMethods

        public static List<string> GetBrowserNames()
        {
            return Value.Browsers.AsQueryable().Select(x => x.Name).ToList();
        }

        public static List<string> GetAltLaunchesNamesForBrowser(string name)
        {
            var selectedBrowser = from browser in Value.Browsers
                                  where browser.Name == name
                                  select browser;
            var altLaunchesStrings = from alt in selectedBrowser.First().AlternateLaunches
                                     select alt.ItemName;

            return altLaunchesStrings.ToList();
        }

        #endregion

        #region RulesetMethods

        public static void AddRuleset(AutoRoutingRules set)
        {
            Value.AutoRoutingRules.Add(set);
            Save();
        }


        #endregion

        private static void Save()
        {
            JsonOperations.FromModelToJson(_instance._settings, Constants.APP_SETTINGS_MAIN);
        }
    }
}
