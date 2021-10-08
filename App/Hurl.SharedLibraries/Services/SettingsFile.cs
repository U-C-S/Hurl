using Hurl.SharedLibraries.Constants;
using Hurl.SharedLibraries.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Hurl.SharedLibraries.Services
{
    public class SettingsFile
    {
        public bool DataExists { get; set; } = false;
        public Settings SettingsObject;

        public SettingsFile()
        {
            if (!File.Exists(MetaStrings.SettingsFilePath))
            {
                Directory.CreateDirectory(OtherStrings.ROAMING + "\\Hurl");

                BrowsersList Browsers = GetBrowsers.FromRegistry();
                SettingsObject = new Settings(Browsers);

                string jsondata = JsonConvert.SerializeObject(SettingsObject, Formatting.Indented);
                File.WriteAllText(MetaStrings.SettingsFilePath, jsondata);
            }
            else
            {
                DataExists = true;
                string jsondata = File.ReadAllText(MetaStrings.SettingsFilePath);
                SettingsObject = JsonConvert.DeserializeObject<Settings>(jsondata);
            }
        }

        public void Update()
        {
            SettingsObject.LastUpdated = DateTime.Now.ToString();
            string jsondata = JsonConvert.SerializeObject(SettingsObject, Formatting.Indented);
            File.WriteAllText(MetaStrings.SettingsFilePath, jsondata);
        }
    }

    // Seperate the setting file creation thing.
    // So that the user need not open the Settings window to actually create the File.
    // It can created from Selection Window.
}
