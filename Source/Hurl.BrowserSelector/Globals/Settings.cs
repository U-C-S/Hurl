using Hurl.Library;
using Hurl.Library.Models;
using System.Collections.Generic;
using System.Windows;

namespace Hurl.BrowserSelector.Globals
{
    public class SettingsGlobal
    {
        private static SettingsGlobal _instance = new();

        private Library.Models.Settings _settings = SettingsFile.GetSettings();

        public static Library.Models.Settings Value
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
            if (Value?.Rulesets == null)
            {
                Value.Rulesets = new List<Ruleset> { };
            }
            Value.Rulesets.Add(new Ruleset() { Rules = rules, BrowserName = name });

            Save();
        }

        public static void AdjustWindowSize(SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width != 0)
            {
                if (Value.AppSettings != null)
                {
                    Value.AppSettings.WindowSize = new int[] { (int)e.NewSize.Width, (int)e.NewSize.Height };
                }
                else
                {
                    Value.AppSettings = new AppSettings() { WindowSize = new int[] { (int)e.NewSize.Width, (int)e.NewSize.Height } };
                }

                Save();
            }

        }

        public static List<Browser> GetBrowsers()
        {
            return Library.GetBrowsers.FromSettingsFile(Value, false);
        }

        public static List<string> GetBrowserNameList()
        {
            return Value.Browsers.ConvertAll(x => x.Name);
        }

        private static void Save()
        {
            JsonOperations.FromModelToJson(_instance._settings, Constants.APP_SETTINGS_MAIN);
        }
    }
}
