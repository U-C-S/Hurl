using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models
{
    public class AutoRoutingRules
    {
        // assign a random number to this
        public int Id { get; set; } = new Random().Next(10000);

        public List<string> Rules { get; set; }

        public string BrowserName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int AltLaunchIndex { get; set; }
    }
}
