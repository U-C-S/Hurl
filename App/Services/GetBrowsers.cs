using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Hurl.Services
{
    /// <summary>
    /// Store all info about a browser
    /// </summary>
    public class BrowserObject
    {
        public string Name;
        public string ExePath;
        public BrowserSourceType SourceType;
        public Icon GetIcon => IconExtractor.FromFile(ExePath.Substring(1, ExePath.Length - 2));
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
                    //Console.WriteLine(x[i]);
                    string name = null;
                    string exepath = null;
                    using (RegistryKey subkey = key.OpenSubKey(x[i] + "\\Capabilities"))
                    {
                        if (subkey != null)
                        {
                            object y = subkey.GetValue("ApplicationName");
                            name = y.ToString();
                            //Console.WriteLine($"{i}. {name}");
                        }
                    }

                    using (RegistryKey subkey = key.OpenSubKey(x[i] + "\\shell\\open\\command"))
                    {
                        if (subkey != null)
                        {
                            object y = subkey.GetValue(null); //to get (Default) value
                            exepath = y.ToString();
                            //Console.WriteLine("-- " + exepath);
                        }
                    }

                    if (name != null & exepath != null)
                    {
                        BrowserObject b = new BrowserObject()
                        {
                            Name = name,
                            ExePath = exepath,
                            SourceType = BrowserSourceType.Registry,
                        };
                        browsers.Add(b);
                    }
                }
            }

            return browsers;
        }

        //public static BrowsersList FromSettings()
        //{

        //}
    }

}
