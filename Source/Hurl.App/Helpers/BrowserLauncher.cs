using Hurl.Library.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hurl.App.Helpers;

internal sealed class BrowserLauncher(
    Func<string, PackagedBrowser?> resolvePackage,
    Func<Uri, string, Task<bool>> activatePackage,
    Action<string, string> startProcess)
{
    public async Task LaunchAsync(string uri, Browser browser, Guid? alternateLaunchId)
    {
        AlternateLaunch? alternate = alternateLaunchId is Guid id
            ? browser.AlternateLaunches?.FirstOrDefault(x => x.Id == id)
                ?? throw new InvalidOperationException("Alternate Launch profile does not exist")
            : null;
        string? arguments = alternate is null ? browser.LaunchArgs : alternate.LaunchArgs;
        if (alternate is null && string.IsNullOrEmpty(arguments)
            && Uri.TryCreate(uri, UriKind.Absolute, out var target)
            && resolvePackage(browser.ExePath) is { } package
            && package.Protocols.Contains(target.Scheme))
        {
            if (!await activatePackage(target, package.FamilyName))
            {
                throw new InvalidOperationException($"Could not open the link in {browser.Name}.");
            }
            return;
        }

        StartProcess(uri, browser.ExePath, arguments);
    }

    public void LaunchAlternate(string uri, Browser browser, AlternateLaunch alternate) =>
        StartProcess(uri, browser.ExePath, alternate.LaunchArgs);

    private void StartProcess(string uri, string executable, string? arguments)
    {
        string expanded = arguments?.Contains("%URL%") == true
            ? arguments.Replace("%URL%", uri)
            : uri + " " + arguments;
        startProcess(executable, expanded);
    }
}
