using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models
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

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LinkPattern[] AutoRules { get; set; }
    }
}
