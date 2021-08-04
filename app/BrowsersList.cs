using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace app
{
    public class BrowsersList: List<Browser>
    {
        public static BrowsersList InitalGetList()
        {
            BrowsersList browsers;
            Console.WriteLine("The list of Browsers: ");

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
                            Console.WriteLine("- " + name);
                        }
                    }

                    using (RegistryKey subkey = key.OpenSubKey(x[i] + "\\shell\\open\\command"))
                    {
                        if (subkey != null)
                        {
                            object y = subkey.GetValue(null); //to get (Default) value
                            exepath = y.ToString();
                            Console.WriteLine("-- " + exepath);
                        }
                    }

                    Browser b = new Browser(name, exepath);
                    browsers.Add(b);
                }
            }

            return browsers;
        }

        public BrowsersList()
        {

        }
    }
}
