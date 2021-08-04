using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace skim
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(RuntimeInformation.OSDescription + " " + RuntimeInformation.OSArchitecture);

            Browserlist.getlist();
            // Console.WriteLine(x);
            // Console.WriteLine(RuntimeInformation.FrameworkDescription);
        }

        class Browserlist
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
}
