using Hurl.Constants;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using TextBox = System.Windows.Controls.TextBox;

namespace Hurl.Services
{
    public class Installer
    {
        public string installLocation;
        public bool isInstalled = false;
        public bool isDefault = false;
        public bool hasProtocol = false;

        public Installer() { Initialize(); }

        private RegistryKey HKCU = Registry.CurrentUser;
        private readonly string startMenuInternet_Key = @"Software\Clients\StartMenuInternet\" + MetaStrings.NAME;
        private readonly string urlAssociate_Key = @"Software\Classes\" + MetaStrings.URLAssociations;
        private readonly string OpenedFrom = Environment.GetCommandLineArgs()[0];

        private void Initialize()
        {
            GetDefaultStatus();
            GetInstallationStatus();
            GetProtocolStatus();
        }

        /// <summary>
        /// To Register the Protocol
        /// i.e here it registers `hurl://` which can be used by Extension
        /// </summary>
        public void ProtocolRegister(bool dontLog = false)
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
            }
        }

        public void SetDefault()
        {
            if (isInstalled)
            {
                Registry.CurrentUser
                    .OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", true)
                    .SetValue("ProgID", MetaStrings.URLAssociations);
            }
            else
            {
                //TODO
            }
        }

        private void GetDefaultStatus()
        {
            var httpDefaultKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", false);
            var Default = httpDefaultKey.GetValue("ProgID");
            isDefault = Default.Equals(MetaStrings.URLAssociations);
        }

        private void GetInstallationStatus()
        {
            var key1 = Registry.CurrentUser.OpenSubKey(startMenuInternet_Key);
            var key2 = Registry.CurrentUser.OpenSubKey(urlAssociate_Key);

            isInstalled = key1 != null && key2 != null;
        }

        private void GetProtocolStatus()
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(MetaStrings.NAME.ToLower());
            hasProtocol = key != null;
        }

        public static bool IsAdministrator
        {
            get
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
