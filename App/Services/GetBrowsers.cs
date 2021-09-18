using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;

namespace Hurl.Services
{
    /// <summary>
    /// Store all info about a browser
    /// </summary>
    public class BrowserObject
    {
        public string Name { get; set; }
        public string ExePath { get; set; }
        public BrowserSourceType SourceType { get; set; }
        public ImageSource GetIcon
        {
            get
            {
                Icon x = ExePath.StartsWith('"'.ToString())
                    ? IconExtractor.FromFile(ExePath.Substring(1, ExePath.Length - 2))
                    : IconExtractor.FromFile(ExePath);

                return IconUtilites.ToImageSource(x);
            }
        }

        public bool Hidden { get; set; } = false;
        public string[] Arguments { get; set; }
        //private string IncognitoArg = null;
    }

    public enum BrowserSourceType
    {
        Registry,
        User
    }

    public class BrowsersList : List<BrowserObject>
    {

    }

    public class GetBrowsers
    {
        public static BrowsersList FromRegistry()
        {
            BrowsersList browsers;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\StartMenuInternet"))
            {
                browsers = new BrowsersList();
                string[] x = key.GetSubKeyNames();
                for (int i = 0; i < x.Length; i++)
                {
                    string Name = null;
                    string ExePath = null;
                    using (RegistryKey subkey = key.OpenSubKey(x[i] + "\\Capabilities"))
                    {
                        if (subkey != null)
                        {
                            string path = subkey.GetValue("ApplicationIcon").ToString();
                            char comma = ',';

                            ExePath = path.Split(comma)[0];
                            Name = subkey.GetValue("ApplicationName").ToString();
                        }
                    }

                    if (Name != null & ExePath != null)
                    {
                        BrowserObject b = new BrowserObject()
                        {
                            Name = Name,
                            ExePath = ExePath,
                            SourceType = BrowserSourceType.Registry,
                        };
                        browsers.Add(b);
                    }
                }
            }

            return browsers;
        }
    }

}
