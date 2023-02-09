using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models
{
    public class AppSettings
    {
        public bool LaunchUnderMouse { get; set; } = false;

        public bool NoWhiteBorder { get; set; } = false;

        public string BackgroundType { get; set; } = "mica";

        public int[] WindowSize { get; set; } = new int[] { 420, 210 };
    }

    public class LinkPattern
    {
        public string Pattern { get; set; }
        public Browser Browser { get; set; }
    }
}
