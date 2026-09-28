using Hurl.Library.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace Hurl.App.Helpers;

class UriLauncher
{
    public static void ResolveAutomatically(string uri, Browser browser, Guid? alternateLaunchId)
    {
        if (alternateLaunchId is Guid id)
        {
            Alternative(uri, browser, id);
        }
        else
        {
            Default(uri, browser);
        }
    }

    public static void Default(string uri, Browser browser)
    {
        Launch(uri, browser.ExePath, browser.LaunchArgs);
    }

    public static void Default(string uri, TransientBrowser browser)
    {
        Launch(uri, browser.ExePath, browser.Arguments);
    }

    public static void Alternative(string uri, Browser browser, Guid alternateLaunchId)
    {
        var alt = browser.AlternateLaunches?.FirstOrDefault(x => x.Id == alternateLaunchId)
            ?? throw new Exception("Alternate Launch profile does not exist");

        Alternative(uri, browser, alt);
    }

    public static void Alternative(string uri, Browser browser, AlternateLaunch alt)
    {
        Launch(uri, browser.ExePath, alt.LaunchArgs);
    }

    private static void Launch(string uri, string executablePath, string? launchArgs)
    {
        string arguments = !string.IsNullOrEmpty(launchArgs) && launchArgs.Contains("%URL%")
            ? launchArgs.Replace("%URL%", uri)
            : uri + " " + launchArgs;

        Process.Start(executablePath, arguments);
    }
}
