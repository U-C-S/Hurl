using System;
using System.Collections.Generic;

namespace Hurl.Library.Models
{
    public class Settings
    {
        public string Version = Constants.VERSION;

        public string LastUpdated { get; set; } = DateTime.Now.ToString();

        public List<Browser> Browsers { get; set; }

        public AppSettings AppSettings { get; set; }

        public List<AutoRoutingRules> AutoRoutingRules { get; set; }
    }
}
