using Hurl.Library;
using Hurl.Library.Models;
using System.Collections.Generic;
using System.Windows;

namespace Hurl.BrowserSelector.State
{
    public class Settings
    {
        private static readonly Settings _instance = new();

        private Library.Models.Settings _settings = Library.Models.Settings.GetSettings();

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

        public static AppSettings AppSettings
        {
            get
            {
                return Value.AppSettings;
            }
        }

        public static List<Browser> Browsers
        {
            get
            {
                return Value.Browsers;
            }
        }

        public static List<Ruleset> Rulesets
        {
            get
            {
                return Value.Rulesets;
            }
        }

        public static void AdjustWindowSize(SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width != 0)
            {
                if (Value.AppSettings != null)
                {
                    Value.AppSettings.WindowSize = [(int)e.NewSize.Width, (int)e.NewSize.Height];
                }
                else
                {
                    Value.AppSettings = new AppSettings() { WindowSize = [(int)e.NewSize.Width, (int)e.NewSize.Height] };
                }

                Save();
            }

        }

        private static void Save()
        {
            JsonOperations.FromModelToJson(Value, Constants.APP_SETTINGS_MAIN);
        }
    }
}
