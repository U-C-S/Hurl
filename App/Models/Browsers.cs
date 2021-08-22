using Microsoft.Win32;
using System.Collections.Generic;
using System.Drawing;

namespace Hurl.Models
{
    /// <summary>
    /// Store all info about a browser
    /// </summary>
    /// <param name="Name">Browser Name</param>
    /// <param name="ExePath">The location of the Executable</param>
    public class BrowserObject
    {
        public string Name;
        public string ExePath;
        //private string IncognitoArg = null;

        public BrowserObject(string Name, string ExePath)
        {
            this.Name = Name;
            this.ExePath = ExePath;
        }

        public Icon GetIcon
        {
            get
            {
                string x = ExePath.Trim().Substring(1, ExePath.Length - 2);
                return Icon.ExtractAssociatedIcon(x);
            }
        }
    }

    public class BList : List<BrowserObject>
    {
        public BList(List<BrowserObject> browsers)
        {

        }

        public BList() { }

        public static BList InitalGetList()
        {
            BList browsers;

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
    }

}
