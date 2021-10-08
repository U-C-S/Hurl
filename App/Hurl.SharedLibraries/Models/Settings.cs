using Hurl.SharedLibraries.Constants;
using Hurl.SharedLibraries.Services;
using System;

namespace Hurl.SharedLibraries.Models
{
    public class Settings
    {
        public string LastUpdated { get; set; } = DateTime.Now.ToString();
        public string AppPath = MetaStrings.SettingsFilePath;
        public BrowsersList Browsers;

        public Settings(BrowsersList browsers)
        {
            Browsers = browsers;
        }
    }
}
