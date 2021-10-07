using Hurl.SharedLibraries.Constants;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Hurl.SharedLibraries.Services
{
    public class SettingsFile
    {
        public bool DataExists { get; set; } = false;
        private BrowsersList Browsers;

        public SettingsFile(BrowsersList browsers)
        {
            this.Browsers = browsers;
            if (!File.Exists(MetaStrings.SettingsFilePath))
            {
                Directory.CreateDirectory(OtherStrings.ROAMING + "\\Hurl");
                this.UpdateFile();
            }
            else
            {
                DataExists = true;
            }
        }

        public SettingsFile() { }

        public SettingsModel ReadSettingsFile()
        {
            string jsondata = File.ReadAllText(MetaStrings.SettingsFilePath);
            return JsonConvert.DeserializeObject<SettingsModel>(jsondata);
        }


        private void UpdateFile()
        {
            string jsondata = JsonConvert.SerializeObject(new SettingsModel(this.Browsers), Formatting.Indented);
            File.WriteAllText(MetaStrings.SettingsFilePath, jsondata);
        }
    }

    // Seperate the setting file creation thing.
    // So that the user need not open the Settings window to actually create the File.
    // It can created from Selection Window.

    public class SettingsModel
    {
        public string lastUpdated { get; set; } = DateTime.Now.ToString();
        public string AppPath = MetaStrings.SettingsFilePath;
        public BrowsersList Browsers;

        public SettingsModel(BrowsersList browsers)
        {
            Browsers = browsers;
        }
    }
}
