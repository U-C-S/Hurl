using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models
{
    public class Settings
    {
        public string Version = Constants.VERSION;

        public string LastUpdated { get; set; } = DateTime.Now.ToString();

        public List<Browser> Browsers { get; set; }

        public AppSettings AppSettings { get; set; }
    }
}
