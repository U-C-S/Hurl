using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class BrowserListViewModel : BaseViewModel
    {
        public List<Browser> Browsers { get; set; }

        public BrowserListViewModel(Settings settings)
        {
            Browsers = GetBrowsers.FromSettingsFile(settings);
        }

        public void OpenLink(Browser clickedbrowser)
        {
            var browser = clickedbrowser;
            var Link = CurrentLink.Value;
            //Process.Start(browser.ExePath, "https://github.com/u-c-s" + " " + browser.LaunchArgs);

            if (!string.IsNullOrEmpty(browser.LaunchArgs) && browser.LaunchArgs.Contains("%URL%"))
            {
                var newArg = browser.LaunchArgs.Replace("%URL%", Link);
                Process.Start(browser.ExePath, newArg);
            }
            else
            {
                Process.Start(browser.ExePath, Link + " " + browser.LaunchArgs);
            }
        }

        public void OpenAltLaunch(AlternateLaunch alt, Browser browser)
        {
            if (alt.LaunchArgs.Contains("%URL%"))
            {
                Process.Start(browser.ExePath, alt.LaunchArgs.Replace("%URL%", CurrentLink.Value));
            }
            else
            {
                Process.Start(browser.ExePath, CurrentLink.Value + " " + alt.LaunchArgs);
            }
        }
    }
}
