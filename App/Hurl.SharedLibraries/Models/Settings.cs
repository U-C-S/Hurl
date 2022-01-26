using Hurl.SharedLibraries.Constants;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hurl.SharedLibraries.Models
{
    public class Settings
    {
        [JsonInclude]
        public string Version = MetaStrings.VERSION;
      
        [JsonInclude]
        public string LastUpdated { get; set; } = DateTime.Now.ToString();
        
        [JsonInclude]
        public string AppPath = MetaStrings.SettingsFilePath;

        [JsonInclude]
        public List<Browser> Browsers;

        public Settings(List<Browser> browsers)
        {
            Browsers = browsers;
        }
    }
}
