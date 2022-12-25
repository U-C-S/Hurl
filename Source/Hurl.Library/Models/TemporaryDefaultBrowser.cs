using System;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models
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
