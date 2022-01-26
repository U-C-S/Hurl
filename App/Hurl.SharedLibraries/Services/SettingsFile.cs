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
        public static SettingsFile LoadNewInstance() => new SettingsFile();

        //public bool DataExists { get; set; } = false;
        // convert this to reuable, so we can use it in other places
        // ex: SettingsFile(string filePath, DataModel)
        //remove this
        public Settings SettingsObject;

        public SettingsFile()
        {
            if (!File.Exists(MetaStrings.SettingsFilePath))
            {
                Directory.CreateDirectory(OtherStrings.ROAMING + "\\Hurl");

                List<Browser> Browsers = GetBrowsers.FromRegistry();
                SettingsObject = new Settings(Browsers);

                string jsondata = JsonSerializer.Serialize(SettingsObject, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true
                });
                File.WriteAllText(MetaStrings.SettingsFilePath, jsondata);
            }
            else
            {
                string jsondata = File.ReadAllText(MetaStrings.SettingsFilePath);
                SettingsObject = JsonSerializer.Deserialize<Settings>(jsondata);
            }
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
