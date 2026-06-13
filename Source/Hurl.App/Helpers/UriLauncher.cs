using Hurl.Library.Models;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.System;

namespace Hurl.App.Helpers;

class UriLauncher
{
    public static void ResolveAutomatically(string uri, Browser browser, Guid? alternateLaunchId)
    {
        if (browser.IsUwp)
        {
            Uwp(uri, browser);
        }
        else if (alternateLaunchId is Guid id)
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
