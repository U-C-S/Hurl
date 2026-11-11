using Hurl.Library.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hurl.App.Helpers;

class UriLauncher
{
    private static readonly BrowserLauncher Launcher = new(
        PackagedBrowserResolver.Resolve,
        LaunchPackageAsync,
        (path, arguments) => Process.Start(path, arguments));

    public static Task ResolveAutomaticallyAsync(string uri, Browser browser, Guid? alternateLaunchId) =>
        Launcher.LaunchAsync(uri, browser, alternateLaunchId);

    private static async Task<bool> LaunchPackageAsync(Uri uri, string family)
    {
        return await global::Windows.System.Launcher.LaunchUriAsync(uri, new global::Windows.System.LauncherOptions
        {
            TargetApplicationPackageFamilyName = family
        });
    }

    public static void Alternative(string uri, Browser browser, AlternateLaunch alt) =>
        Launcher.LaunchAlternate(uri, browser, alt);
}
