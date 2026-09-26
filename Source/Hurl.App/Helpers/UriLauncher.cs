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
        if (!string.IsNullOrEmpty(browser.LaunchArgs) && browser.LaunchArgs.Contains("%URL%"))
        {
            var newArg = browser.LaunchArgs.Replace("%URL%", uri);
            Process.Start(browser.ExePath, newArg);
        }
        else
        {
            Process.Start(browser.ExePath, uri + " " + browser.LaunchArgs);
        }
    }

    public static void Alternative(string uri, Browser browser, Guid alternateLaunchId)
    {
        var alt = browser.AlternateLaunches?.FirstOrDefault(x => x.Id == alternateLaunchId)
            ?? throw new Exception("Alternate Launch profile does not exist");

        Alternative(uri, browser, alt);
    }

    public static void Alternative(string uri, Browser browser, AlternateLaunch alt)
    {
        if (alt.LaunchArgs.Contains("%URL%"))
        {
            var args = alt.LaunchArgs.Replace("%URL%", uri);
            Process.Start(browser.ExePath, args);
        }
        else
        {
            var args = uri + " " + alt.LaunchArgs;
            Process.Start(browser.ExePath, args);
        }
    }
}
