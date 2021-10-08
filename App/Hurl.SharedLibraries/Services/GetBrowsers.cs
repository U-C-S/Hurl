using Hurl.SharedLibraries.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace Hurl.SharedLibraries.Services
{
    public class GetBrowsers
    {
        public static List<Browser> FromRegistry()
        {
            List<Browser> browsers;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\StartMenuInternet"))
            {
                browsers = new List<Browser>();
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
                        Browser b = new Browser(Name, ExePath)
                        {
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
