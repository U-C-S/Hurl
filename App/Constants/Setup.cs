using Microsoft.Win32;
using System;
using System.IO;

namespace Hurl.Constants
{
    public class Setup
    {
        private static RegistryKey root = Registry.CurrentUser;
        private static readonly string startMenuInternet_Key = @"Software\Clients\StartMenuInternet\" + Constants.NAME;
        private static readonly string urlAssociate_Key = @"Software\Classes\" + Constants.URLAssociations;
        private string InstallLocation = Environment.GetCommandLineArgs()[0];
        private string OpenedFrom = Environment.GetCommandLineArgs()[0];

        /// <summary>
        /// Installs the tool
        /// </summary>
        public void Install(string InstallPath, System.Windows.Controls.TextBox LogBox)
        {
            Uninstall();
            LogBox.Text += "Removed the Traces of previous installation from Registry";
            
            if (!InstallPath.Equals(""))
            {
                InstallLocation = InstallPath + "\\Hurl.exe";
            }

            File.Copy(OpenedFrom, InstallLocation, true);


            using (RegistryKey key = root.CreateSubKey(startMenuInternet_Key))
            {
                key.SetValue(null, Constants.NAME);

                using (RegistryKey Cap = key.CreateSubKey("Capabilities"))
                {
                    Cap.SetValue("ApplicationName", Constants.NAME);
                    Cap.SetValue("ApplicationDescription", Constants.DESCRIPTION);
                    Cap.SetValue("ApplicationIcon", $"{InstallLocation},0"); //change

                    RegistryKey sm = Cap.CreateSubKey("StartMenu");
                    sm.SetValue("StartMenuInternet", Constants.NAME);

                    RegistryKey ua = Cap.CreateSubKey("URLAssociations");
                    ua.SetValue("http", Constants.URLAssociations);
                    ua.SetValue("https", Constants.URLAssociations);
                }

                key.CreateSubKey("DefaultIcon").SetValue(null, $"{InstallLocation},0"); //change

                key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{InstallLocation}\""); //change
            }

            root.OpenSubKey(@"Software\RegisteredApplications", true).SetValue(Constants.NAME, $"Software\\Clients\\StartMenuInternet\\{Constants.NAME}\\Capabilities");

            using (RegistryKey key = root.CreateSubKey(urlAssociate_Key))
            {
                key.SetValue(null, $"{Constants.NAME} URL");
                key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{InstallLocation}\" \"%1\"");  //change
            }

            LogBox.Text += "installated Successfully";
        }



        /// <summary>
        /// For Uninstalling the software from Registry
        /// </summary>
        public static void Uninstall()
        {
            root.DeleteSubKeyTree(startMenuInternet_Key, false);
            root.DeleteSubKeyTree(urlAssociate_Key, false);
            root.OpenSubKey(@"Software\RegisteredApplications", true).DeleteValue(Constants.NAME, false);
        }
    }
}
