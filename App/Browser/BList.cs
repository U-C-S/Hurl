using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace Hurl.Browser
{
    public class BList : List<BrowserObject>
    {
        public static BList InitalGetList()
        {
            BList browsers;
            Console.WriteLine("The list of Browsers: ");

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\StartMenuInternet"))
            {
                browsers = new BList();
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
                        BrowserObject b = new BrowserObject(name, exepath);
                        browsers.Add(b);
                    }
                }
            }

            return browsers;
        }


        public BList(List<BrowserObject> browsers)
        {

        }

        public BList() { }
    }

    public class BrowserObject
    {
        public string Name { get; set; }
        public string ExePath { get; set; }


        public BrowserObject(string name, string exePath)
        {
            Name = name;
            ExePath = exePath;
        }
    }
}
