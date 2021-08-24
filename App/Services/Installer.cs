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
        private Logger log;

        public Installer(TextBox LogBox)
        {
            log = new Logger(LogBox);
        }

        public Installer() { }

        private void Initialize()
        {

        }

        private RegistryKey HKCU = Registry.CurrentUser;
        private readonly string startMenuInternet_Key = @"Software\Clients\StartMenuInternet\" + MetaStrings.NAME;
        private readonly string urlAssociate_Key = @"Software\Classes\" + MetaStrings.URLAssociations;
        private readonly string OpenedFrom = Environment.GetCommandLineArgs()[0];

        /// <summary>
        /// Installs the tool
        /// </summary>
        public void Install(string InstallPath)
        {
            log.Start("Install");
            Uninstall();
            log.write("Removed the Traces of previous installation from Registry");

            if (!InstallPath.Equals(""))
            {
                installLocation = InstallPath + "\\Hurl.exe";
                log.write("Installation location: " + installLocation);
            }

            //Add to registry code starts from here
            try
            {
                File.Copy(OpenedFrom, installLocation, true);
                log.write($"Copied file from {OpenedFrom} to install Location.");

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

                    log.write($"Added the Subkey: {key} to Registry");
                }

                HKCU.OpenSubKey(@"Software\RegisteredApplications", true).SetValue(MetaStrings.NAME, $"Software\\Clients\\StartMenuInternet\\{MetaStrings.NAME}\\Capabilities");

                using (RegistryKey key = HKCU.CreateSubKey(urlAssociate_Key))
                {
                    key.SetValue(null, $"{MetaStrings.NAME} URL");
                    key.CreateSubKey(@"shell\open\command").SetValue(null, $"\"{installLocation}\" \"%1\"");  //change

                    log.write($"Added the Subkey: {key} to Registry");
                }

                // FOR PROTOCOL REGISTRING
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

                log.Stop();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                //TODO
            }
        }

        /// <summary>
        /// For Uninstalling the software from Registry
        /// </summary>
        public void Uninstall()
        {
            HKCU.DeleteSubKeyTree(startMenuInternet_Key, false);
            HKCU.DeleteSubKeyTree(urlAssociate_Key, false);
            HKCU.OpenSubKey(@"Software\RegisteredApplications", true).DeleteValue(MetaStrings.NAME, false);

            Registry.ClassesRoot.DeleteSubKeyTree(MetaStrings.NAME.ToLower(), false);
        }

        public void GetDefaultBrowserStatus()
        {

        }

        public void GetInstallationStatus()
        {

        }

        // Future: Maybe just dont register the protocol, instead of preventing the user from installing
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
