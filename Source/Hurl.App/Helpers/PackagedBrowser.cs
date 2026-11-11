using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Hurl.App.Helpers;

internal sealed record PackagedBrowser(string FamilyName, string? LogoPath, HashSet<string> Protocols)
{
    public static PackagedBrowser? FromManifest(XDocument manifest, string directory, string family, string alias)
    {
        var application = manifest.Descendants()
            .Where(x => x.Name.LocalName == "Application")
            .SingleOrDefault(app => app.Descendants().Any(x =>
                x.Name.LocalName == "ExecutionAlias"
                && string.Equals((string?)x.Attribute("Alias"), alias, StringComparison.OrdinalIgnoreCase)));
        if (application is null) return null;

        var protocols = application.Descendants()
            .Where(x => x.Name.LocalName == "Protocol")
            .Select(x => (string?)x.Attribute("Name"))
            .OfType<string>()
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        string? logo = application.Descendants().FirstOrDefault(x => x.Name.LocalName == "VisualElements")
            ?.Attribute("Square44x44Logo")?.Value;
        return new(family, ResolveLogo(directory, logo), protocols);
    }

    private static string? ResolveLogo(string directory, string? logo)
    {
        if (string.IsNullOrWhiteSpace(logo)) return null;
        string root = Path.GetFullPath(directory) + Path.DirectorySeparatorChar;
        string path = Path.GetFullPath(Path.Combine(directory, logo.Replace('\\', Path.DirectorySeparatorChar)));
        if (!path.StartsWith(root, StringComparison.OrdinalIgnoreCase)) return null;

        string stem = Path.Combine(Path.GetDirectoryName(path)!, Path.GetFileNameWithoutExtension(path));
        string extension = Path.GetExtension(path);
        foreach (int size in new[] { 256, 128, 64, 48, 44, 32, 24, 16 })
        {
            foreach (string suffix in new[] { "_altform-unplated", "" })
            {
                string candidate = $"{stem}.targetsize-{size}{suffix}{extension}";
                if (File.Exists(candidate)) return candidate;
            }
        }

        if (File.Exists(path)) return path;
        foreach (int scale in new[] { 400, 300, 200, 150, 125, 100 })
        {
            string candidate = $"{stem}.scale-{scale}{extension}";
            if (File.Exists(candidate)) return candidate;
        }
        return null;
    }
}
