using System.IO;
using Hurl.Constants;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Hurl.Services.AppSettings
{
    internal class SettingsFile
    {
        public bool DataExists { get; set; } = false;

        public SettingsFile()
        {
            if (!File.Exists(MetaStrings.SettingsFilePath))
            {
                Directory.CreateDirectory(OtherStrings.ROAMING + "\\Hurl");
                string jsondata = JsonConvert.SerializeObject(new SettingsModel());
                File.WriteAllText(MetaStrings.SettingsFilePath, jsondata);
            }
            else
            {
                
            }
        }
    }

    public class SettingsModel
    {
        public string AppPath = MetaStrings.SettingsFilePath;
        public BrowsersList Browsers;
    }
}
