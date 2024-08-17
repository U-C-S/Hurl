using Hurl.Library.Models;
using Microsoft.Win32;
using System.IO;

namespace Hurl.Library;

public static class GetBrowsers
{
    public static List<Browser> FromRegistry()
    {
        List<Browser> browsers = [];

        using (var key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\StartMenuInternet"))
        {
            string[] x = key?.GetSubKeyNames() ?? [];
            for (int i = 0; i < x.Length; i++)
            {
                string? Name = null;
                string? ExePath = null;
                using (var subkey = key?.OpenSubKey(x[i] + "\\Capabilities"))
                {
                    if (subkey is RegistryKey capabilitiesKey)
                    {
                        var path = capabilitiesKey.GetValue("ApplicationIcon")?.ToString();
                        var comma = ',';

                        ExePath = path?.Split(comma)[0];
                        Name = capabilitiesKey.GetValue("ApplicationName")?.ToString();
                    }
                }

                if (Name is string _Name && ExePath is string _ExePath)
                {
                    Browser b = new(_Name, _ExePath);
                    browsers.Add(b);
                }
            }
        }

        return browsers;
    }

    public static List<Browser> FromSettingsFile(Settings settings, bool includeHidden = false)
    {
        return (from b in settings.Browsers
                where b.Name != null && b.ExePath != null && b.Hidden != true
                select b).ToList();
    }
}
