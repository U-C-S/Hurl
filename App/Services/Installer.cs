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
        /// Installs the tool by -
        /// Registering it directly into the Windows Registry
        /// </summary>
        public void Install(string InstallPath, bool dontLog = false)
        {
            Uninstall(true);

            if (!InstallPath.Equals(""))
            {
                installLocation = InstallPath + "\\Hurl.exe";
            }

            int stage = 0;
            //Add to registry code starts from here
            try
            {
                stage = 1;
                File.Copy(OpenedFrom, installLocation, true);

                stage = 2;
                using (RegistryKey key = HKCU.CreateSubKey(startMenuInternet_Key))
                {
                    key.SetValue(null, MetaStrings.NAME);

                    using (RegistryKey Cap = key.CreateSubKey("Capabilities"))
                    {
                        Cap.SetValue("ApplicationName", MetaStrings.NAME);
                        Cap.SetValue("ApplicationDescription", MetaStrings.DESCRIPTION);
                        Cap.SetValue("ApplicationIcon", $"{installLocation},0"); //change

                        RegistryKey sm = Cap.CreateSubKey("StartMenu");
                        sm.SetValue("StartMenuInternet", MetaStrings.NAME);

                        RegistryKey ua = Cap.CreateSubKey("URLAssociations");
                        ua.SetValue("http", MetaStrings.URLAssociations);
                        ua.SetValue("https", MetaStrings.URLAssociations);
                    }

                    key.CreateSubKey("DefaultIcon").SetValue(null, $"{installLocation},0"); //change

                    key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{installLocation}\""); //change
                }

                stage++;
                HKCU.OpenSubKey(@"Software\RegisteredApplications", true)
                    .SetValue(MetaStrings.NAME, $"Software\\Clients\\StartMenuInternet\\{MetaStrings.NAME}\\Capabilities");

                stage++;
                using (RegistryKey key = HKCU.CreateSubKey(urlAssociate_Key))
                {
                    key.SetValue(null, $"{MetaStrings.NAME} URL");
                    key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{installLocation}\" \"%1\"");  //change
                }
            }
            catch (Exception err)
            {
                if (stage >= 1) File.Delete(installLocation);
                if (stage >= 2) Uninstall();
            }
        }

        /// <summary>
        /// For Uninstalling the software from Registry
        /// </summary>
        public void Uninstall(bool dontLog = false)
        {
            void RemoveNonAdminKeys()
            {
                HKCU.DeleteSubKeyTree(startMenuInternet_Key, false);

                HKCU.DeleteSubKeyTree(urlAssociate_Key, false);

                HKCU.OpenSubKey(@"Software\RegisteredApplications", true).DeleteValue(MetaStrings.NAME, false);
            }

            // Need to be in admin mode to remove protocol
            if (hasProtocol)
            {
                if (IsAdministrator)
                {
                    RemoveNonAdminKeys();

                    Registry.ClassesRoot.DeleteSubKeyTree(MetaStrings.NAME.ToLower(), false);
                }
            }
            else
            {
                RemoveNonAdminKeys();
            }

            //log.Stop();

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
