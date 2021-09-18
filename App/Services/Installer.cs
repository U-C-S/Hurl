using Hurl.Constants;
using Microsoft.Win32;
using System;
using System.Security.Principal;
using System.Windows;

namespace Hurl.Services
{
    public class Installer
    {
        public string installLocation;

        public Installer()
        {
            installLocation = IsInstalled();
        }

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

        private string IsInstalled()
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
            if (IsAdministrator)
            {
                string Name_lower = MetaStrings.NAME.ToLower();

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(Name_lower, true))
                {
                    key.SetValue(null, $"URL:{Name_lower}");
                    key.SetValue("URL Protocol", "");
                    key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{installLocation}\" \"%1\"");  //change
                }
                return true;
            }
            else
            {
                _ = MessageBox.Show("Run the App as Adminstrator");
                return false;
            }
        }

        public bool SetDefault()
        {
            if (installLocation != null)
            {
                Registry.CurrentUser
                    .OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", true)
                    .SetValue("ProgID", MetaStrings.URLAssociations);
                return true;
            }
            else
            {
                MessageBox.Show("Hurl is not even Installed");
                return false;
            }
        }
    }
}
