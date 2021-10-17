using Hurl.SharedLibraries.Constants;
using System;
using System.Collections.Generic;

namespace Hurl.SharedLibraries.Models
{
    public class Settings
    {
        public string LastUpdated { get; set; } = DateTime.Now.ToString();
        public string AppPath = MetaStrings.SettingsFilePath;
        public List<Browser> Browsers;

        public Settings(List<Browser> browsers)
        {
            Browsers = browsers;
        }
    }
}
