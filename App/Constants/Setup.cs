using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace Hurl.Constants
{
    static class Setup
    {
        private static RegistryKey root = Registry.CurrentUser;
        private static readonly string startMenuInternet_Key = @"Software\Clients\StartMenuInternet\" + Constants.NAME;
        private static readonly string urlAssociate_Key = @"Software\Classes\" + Constants.URLAssociations;
        private static string Location = @"D:\lolx\Hurl.exe";

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

            root.OpenSubKey(@"Software\RegisteredApplications", true)!.SetValue(Constants.NAME, @"Software\Clients\StartMenuInternet\Hurl\Capabilities");

            using (RegistryKey key = root.CreateSubKey(urlAssociate_Key))
            {
                key.SetValue(null, $"{Constants.NAME} URL");
                key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{Location}\" \"%1\"");  //change
            }
        }

        /// <summary>
        /// For Uninstalling the software from Registry
        /// </summary>
        public static void Uninstall()
        {
            Debug.WriteLine("Uninstalled the exe");

            root.DeleteSubKeyTree(startMenuInternet_Key, false);
            root.DeleteSubKeyTree(urlAssociate_Key, false);
            root.OpenSubKey(@"Software\RegisteredApplications", true)!.DeleteValue(Constants.NAME, false);
        }
    }
}


/*

REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities /v ApplicationName /d Hurl
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities /v ApplicationDescription /d "Hurl -- select browser dynamically"
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities  /v ApplicationIcon /d "D:\lolx\Hurl.exe,0"

REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities\StartMenu  /v StartMenuInternet /d Hurl
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities\URLAssociations /v http /d HandleURL3721
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\Capabilities\URLAssociations /v https /d HandleURL3721
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\DefaultIcon /ve /d "D:\lolx\Hurl.exe,0"
REG ADD HKCU\Software\Clients\StartMenuInternet\Hurl\shell\open\command /ve /d "D:\lolx\Hurl.exe"

# ;register capablities
REG ADD HKCU\Software\RegisteredApplications /v Hurl /d "Software\Clients\StartMenuInternet\Hurl\Capabilities"

# ;register handler
REG ADD HKCU\Software\Classes\HandleURL3721 /ve /d Hurl Url
# REG ADD HKCU\Software\Classes\HandleURL3721\shell\open\command /ve /d "D:\lolx\Hurl.exe"

$value = "`"D:\lolx\Hurl.exe`" `"%1`""
New-ItemProperty -Path "HKCU\Software\Classes\HandleURL3721\shell\open\command" ` -Name " " ` -Value $value `

*/