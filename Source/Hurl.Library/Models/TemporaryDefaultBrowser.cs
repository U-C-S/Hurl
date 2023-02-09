using System;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models
{
    public class TemporaryDefaultBrowser
    {
        public Browser TargetBrowser { get; set; }

        public DateTime SelectedAt { get; set; }

        public DateTime ValidTill { get; set; }
    }
}
