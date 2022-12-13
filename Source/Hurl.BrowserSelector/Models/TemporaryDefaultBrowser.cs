using System;
using System.Text.Json.Serialization;

namespace Hurl.BrowserSelector.Models
{
    public class TemporaryDefaultBrowser
    {
        [JsonInclude]
        public Browser TargetBrowser { get; set; }

        [JsonInclude]
        public DateTime SelectedAt { get; set; }

        [JsonInclude]
        public DateTime ValidTill { get; set; }
    }
}
