using System;

namespace app.Browser
{
    public class Browser
    {
        public string Name { get; set; }
        public string ExePath { get; set; }


        public Browser(string name, string exePath)
        {
            Name = name;
            ExePath = exePath;
        }
    }
}
