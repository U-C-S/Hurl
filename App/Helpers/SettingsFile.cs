using Hurl.BrowserSelector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Hurl.BrowserSelector.Helpers
{
    public class SettingsFile
    {
        // convert this to reuable, so we can use it in other places
        // ex: SettingsFile(string filePath, DataModel)

        public Settings SettingsObject;

        private SettingsFile(Settings settings)
        {
            this.SettingsObject = settings;
        }

        public static SettingsFile TryLoading()
        {
            return new SettingsFile(GetSettings());
        }

        public static Settings GetSettings()
        {
            string jsondata = File.ReadAllText(Constants.SettingsFilePath);
            var SettingsObject = JsonSerializer.Deserialize<Settings>(jsondata);
            return SettingsObject;
        }

        public static SettingsFile New(List<Browser> browsers)
        {
            Directory.CreateDirectory(Constants.ROAMING + "\\Hurl");

            var _settings = new Settings()
            {
                Browsers = browsers,
            };

            string jsondata = JsonSerializer.Serialize(_settings, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            });
            File.WriteAllText(Constants.SettingsFilePath, jsondata);

            return new SettingsFile(_settings);
        }

        public void Update()
        {
            SettingsObject.LastUpdated = DateTime.Now.ToString();
            string jsondata = JsonSerializer.Serialize(SettingsObject, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            });
            File.WriteAllText(Constants.SettingsFilePath, jsondata);
        }
    }
}
