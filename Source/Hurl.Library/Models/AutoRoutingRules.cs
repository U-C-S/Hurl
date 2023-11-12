using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models
{
    public class AutoRoutingRules
    {
        public List<string> Rules { get; set; }

        public string BrowserName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int AltLaunchIndex { get; set; }
    }
}
