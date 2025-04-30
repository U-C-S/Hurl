using Hurl.Selector.Models;
using System;
using System.Diagnostics;
using Windows.System;

namespace Hurl.BrowserSelector.Helpers;

class UriLauncher
{
    public static void ResolveAutomatically(string uri, Browser browser, int? altLaunchIndex)
    {
        if (browser.IsUwp)
        {
            Uwp(uri, browser);
        }
        else if (altLaunchIndex is not null)
        {
            if (browser.AlternateLaunches?.Count - 1 < altLaunchIndex) // 
                throw new Exception("Alternate Launch profile does not exist");
            else
                Alternative(uri, browser, (int)altLaunchIndex);
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

    public static void Alternative(string uri, Browser browser, int altLaunchIndex)
    {
        var alt = (browser?.AlternateLaunches?[altLaunchIndex]) ?? throw new Exception("Alternate Launch profile does not exist");
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

    public static async void Uwp(string uri, Browser browser)
    {
        var uri_object = new Uri(uri);
        var options = new LauncherOptions
        {
            //options.TargetApplicationPackageFamilyName = "TheBrowserCompany.Arc_ttt1ap7aakyb4";
            //TargetApplicationPackageFamilyName = "Mozilla.Firefox_n80bbvh6b1yt2"
            TargetApplicationPackageFamilyName = browser.ExePath
        };
        await Launcher.LaunchUriAsync(uri_object, options);
    }
}

