using System;
using Microsoft.Win32;

namespace app
{
    public class browserlist
    {
        public static void getlist()
        {
            Console.WriteLine("The list of Browsers: ");
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\StartMenuInternet"))
            {
                string[] x = key.GetSubKeyNames();
                for (int i = 0; i < x.Length; i++)
                {
                    //Console.WriteLine(x[i]);
                    using (RegistryKey subkey = key.OpenSubKey(x[i] + "\\Capabilities"))
                    {
                        if (subkey != null)
                        {
                            object y = subkey.GetValue("ApplicationName");
                            Console.WriteLine("- " + y.ToString());
                        }
                    }
                }

            }
        }
    }
}
