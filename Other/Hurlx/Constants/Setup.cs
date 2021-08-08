using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurlx.Constants
{
    static class Setup
    {
        private static RegistryKey root = Registry.CurrentUser;
        private static readonly string startMenuInternet_Key = @"Software\Clients\StartMenuInternet\" + Constants.NAME;
        private static readonly string urlAssociate_Key = @"Software\Classes\" + Constants.URLAssociations;
        private static string Location = Environment.GetCommandLineArgs()[0];

        /// <summary>
        /// Installs the tool
        /// </summary>
        public static void Install()
        {
            using (RegistryKey key = root.CreateSubKey(startMenuInternet_Key))
            {
                key.SetValue(null, Constants.NAME);

                using (RegistryKey Cap = key.CreateSubKey("Capabilities"))
                {
                    Cap.SetValue("ApplicationName", Constants.NAME);
                    Cap.SetValue("ApplicationDescription", Constants.DESCRIPTION);
                    Cap.SetValue("ApplicationIcon", $"{Location},0"); //change

                    RegistryKey sm = Cap.CreateSubKey("StartMenu");
                    sm.SetValue("StartMenuInternet", Constants.NAME);

                    RegistryKey ua = Cap.CreateSubKey("URLAssociations");
                    ua.SetValue("http", Constants.URLAssociations);
                    ua.SetValue("https", Constants.URLAssociations);
                }

                key.CreateSubKey("DefaultIcon").SetValue(null, $"{Location},0"); //change

                key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{Location}\""); //change
            }

            root.OpenSubKey(@"Software\RegisteredApplications", true).SetValue(Constants.NAME, @"Software\Clients\StartMenuInternet\Hurl\Capabilities");

            using (RegistryKey key = root.CreateSubKey(urlAssociate_Key))
            {
                key.SetValue(null, $"{Constants.NAME} URL");
                key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{Location}\" \"%1\"");  //change
            }

            Debug.WriteLine("Installed the EXE");
        }

        /// <summary>
        /// For Uninstalling the software from Registry
        /// </summary>
        public static void Uninstall()
        {
            root.DeleteSubKeyTree(startMenuInternet_Key, false);
            root.DeleteSubKeyTree(urlAssociate_Key, false);
            root.OpenSubKey(@"Software\RegisteredApplications", true).DeleteValue(Constants.NAME, false);

            Debug.WriteLine("Uninstalled the EXE");
        }
    }
}
