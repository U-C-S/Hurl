using Hurl.SharedLibraries.Constants;
using Hurl.SharedLibraries.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Hurl.SharedLibraries.Services
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
            if (!File.Exists(MetaStrings.SettingsFilePath))
            {
                throw new FileNotFoundException();
            }

            string jsondata = File.ReadAllText(MetaStrings.SettingsFilePath);
            var SettingsObject = JsonSerializer.Deserialize<Settings>(jsondata);
            return SettingsObject;
        }

        public static SettingsFile New(List<Browser> browsers)
        {
            Directory.CreateDirectory(OtherStrings.ROAMING + "\\Hurl");

            var _settings = new Settings()
            {
                Browsers = browsers,
                AppSettings = new AppSettings()
                {
                    DisableAcrylic = false,
                }
            };

            string jsondata = JsonSerializer.Serialize(_settings, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            });
            File.WriteAllText(MetaStrings.SettingsFilePath, jsondata);

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
            File.WriteAllText(MetaStrings.SettingsFilePath, jsondata);
        }
    }
}
