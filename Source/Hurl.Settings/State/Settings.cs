using Hurl.Library;
using Hurl.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hurl.Settings.State
{
    public partial class Settings
    {
        private static Settings _instance = new();

        private Library.Models.Settings _data = SettingsFile.GetSettings();

        private static Library.Models.Settings Value
        {
            get
            {
                return _instance._data;
            }
            set
            {
                _instance._data = value;
            }
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
        public static List<Browser> GetBrowsers()
        {
            return Library.GetBrowsers.FromSettingsFile(Value, false);
        }

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
        public static List<Ruleset> GetAutoRoutingRules()
        {
            return Value.Rulesets;
        }

        public static void AddRuleset(Ruleset set)
        {
            Value.Rulesets.Add(set);
            Save();
        }

        internal static List<Ruleset> MoveRulesetUp(int id)
        {
            var ruleset = Value.Rulesets.Where(x => x.Id == id).First();
            var index = Value.Rulesets.IndexOf(ruleset);

            if (index > 0)
            {
                Value.Rulesets.Remove(ruleset);
                Value.Rulesets.Insert(index - 1, ruleset);
            }

            Save();
            return Value.Rulesets;
        }

        internal static void MoveRulesetDown(int id)
        {
            var ruleset = Value.Rulesets.Where(x => x.Id == id).First();
            var index = Value.Rulesets.IndexOf(ruleset);

            if (index < Value.Rulesets.Count - 1)
            {
                Value.Rulesets.Remove(ruleset);
                Value.Rulesets.Insert(index + 1, ruleset);
            }

            Save();
        }

        internal static void DeleteRuleset(int id)
        {
            var ruleset = Value.Rulesets.Where(x => x.Id == id).First();
            Value.Rulesets.Remove(ruleset);

            Save();
        }

        #endregion

        private static void Save()
        {
            JsonOperations.FromModelToJson(_instance._data, Constants.APP_SETTINGS_MAIN);
        }
    }
}
