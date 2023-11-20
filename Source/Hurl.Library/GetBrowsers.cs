using Hurl.Library.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hurl.Library;

public static class GetBrowsers
{
    public static List<Browser> FromRegistry()
    {
        List<Browser> browsers = null;

        using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\StartMenuInternet"))
        {
            browsers = new List<Browser>();
            string[] x = key.GetSubKeyNames();
            for (int i = 0; i < x.Length; i++)
            {
                string Name = null;
                string ExePath = null;
                using (RegistryKey subkey = key.OpenSubKey(x[i] + "\\Capabilities"))
                {
                    if (subkey != null)
                    {
                        string path = subkey.GetValue("ApplicationIcon").ToString();
                        char comma = ',';

                        ExePath = path.Split(comma)[0];
                        Name = subkey.GetValue("ApplicationName").ToString();
                    }
                }

                if (Name != null & ExePath != null)
                {
                    Browser b = new(Name, ExePath);
                    browsers.Add(b);
                }
            }
        }

        return browsers;
    }

    public static List<Browser> FromSettingsFile(bool includeHidden = false)
    {
        List<Browser> _browsersList = null;

        try
        {
            var _settingsFile = SettingsFile.TryLoading();
            _browsersList = _settingsFile.SettingsObject.Browsers;
        }
        catch (FileNotFoundException)
        {
            _browsersList = FromRegistry();
            SettingsFile.New(_browsersList);
        }

        return (from b in _browsersList
                where b.Name != null && b.ExePath != null && b.Hidden != true
                select b).ToList();
    }

    public static List<Browser> FromSettingsFile(Settings settings, bool includeHidden = false)
    {
        return (from b in settings.Browsers
                where b.Name != null && b.ExePath != null && b.Hidden != true
                select b).ToList();
    }
}
