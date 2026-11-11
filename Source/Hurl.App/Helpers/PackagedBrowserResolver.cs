using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using Windows.Management.Deployment;

namespace Hurl.App.Helpers;

internal static class PackagedBrowserResolver
{
    public static PackagedBrowser? Resolve(string executable)
    {
        if (string.IsNullOrWhiteSpace(executable)) return null;

        string path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(executable.Trim().Trim('"')));
        string aliases = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Microsoft", "WindowsApps");
        if (!string.Equals(Path.GetDirectoryName(path), aliases, StringComparison.OrdinalIgnoreCase)
            || !File.Exists(path)
            || (File.GetAttributes(path) & FileAttributes.ReparsePoint) == 0)
        {
            return null;
        }

        PackagedBrowser? result = null;
        foreach (var package in new PackageManager().FindPackagesForUser(string.Empty))
        {
            if (package.IsFramework || package.IsResourcePackage) continue;
            try
            {
                string directory = package.InstalledLocation.Path;
                var manifest = XDocument.Load(Path.Combine(directory, "AppxManifest.xml"));
                var candidate = PackagedBrowser.FromManifest(manifest, directory, package.Id.FamilyName,
                    Path.GetFileName(path));
                if (candidate is null) continue;
                if (result is not null)
                {
                    Debug.WriteLine($"Multiple installed packages declare execution alias '{path}'.");
                    return null;
                }
                result = candidate;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or XmlException or COMException)
            {
                Debug.WriteLine($"Could not inspect package {package.Id.FamilyName}: {ex.Message}");
            }
        }
        return result;
    }
}
