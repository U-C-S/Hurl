using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hurl.BrowserSelector.Models
{
    public class Settings
    {
        [JsonInclude]
        public string Version = Constants.VERSION;

        [JsonInclude]
        public string LastUpdated { get; set; } = DateTime.Now.ToString();

        [JsonInclude]
        public List<Browser> Browsers;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AppSettings AppSettings { get; set; }
    }

    public class AppSettings
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool DisableAcrylic { get; set; } = false;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Byte> BackgroundRGB { get; set; } = new List<byte> { 51, 51, 51 };

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool LaunchUnderMouse { get; set; } = false;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool UseWhiteBorder { get; set; } = true;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string BackgroundType { get; set; } = "mica";
    }

    public class AppAutoSettings
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int[] WindowSize { get; set; }
    }
}
