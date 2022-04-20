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
        public List<Browser> Browsers;

        [JsonInclude]
        public AppSettings AppSettings { get; set; }

        //public Settings(List<Browser> browsers)
        //{
        //    Browsers = browsers;
        //}
    }

    public class AppSettings
    {
        [JsonInclude]
        public bool DisableAcrylic { get; set; } = false;

        [JsonInclude]
        public List<Byte> BackgroundRGB { get; set; } = new List<byte> { 51, 51, 51 };

    }
}
