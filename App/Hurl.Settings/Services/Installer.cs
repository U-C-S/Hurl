using Hurl.SharedLibraries.Constants;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;

namespace Hurl.Settings.Services
{
    public class Installer
    {
        public bool IsDefault
        {
            get
            {
                var httpDefaultKey = Registry.CurrentUser
                    .OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", false)
                    .GetValue("ProgID");

                return httpDefaultKey.Equals(MetaStrings.URLAssociations);
            }
        }

        private string InstallLocation
        {
            get
            {
                try
                {
                    string startMenuInternet_Key = @"Software\Clients\StartMenuInternet\" + MetaStrings.NAME + @"\Capabilities";
                    string urlAssociate_Key = @"Software\Classes\" + MetaStrings.URLAssociations;

                    var key1 = Registry.CurrentUser.OpenSubKey(startMenuInternet_Key).GetValue("ApplicationIcon");
                    var key2 = Registry.CurrentUser.OpenSubKey(urlAssociate_Key);

                    if (key1 != null && key2 != null)
                        return key1.ToString().Split(',')[0];
                    else
                        return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public bool HasProtocol
        {
            get
            {
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(MetaStrings.NAME.ToLower());
                return key != null;
            }
        }

        public bool IsAdministrator
        {
            get
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// To Register the Protocol
        /// i.e here it registers `hurl://` which can be used by Extension
        /// </summary>
        public bool ProtocolRegister()
        {
            if (IsAdministrator && InstallLocation != null)
            {
                string Name_lower = MetaStrings.NAME.ToLower();

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(Name_lower, true))
                {
                    key.SetValue(null, $"URL:{Name_lower}");
                    key.SetValue("URL Protocol", "");
                    key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{InstallLocation}\" \"%1\"");  //change
                }
                return true;
            }
            else
            {
                _ = MessageBox.Show("ERROR! Either App not Installed or Run the App as Admin");
                return false;
            }
        }

        public void SetDefault()
        {
            if (InstallLocation != null)
            {
                Process.Start("ms-settings:defaultapps");
            }
            else
            {
                MessageBox.Show("Hurl is not even Installed");
            }
        }
    }
}
