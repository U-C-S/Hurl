using Hurl.BrowserSelector.Globals;
using Hurl.Library;
using Hurl.Library.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class BrowserListViewModel : BaseViewModel
    {
        public List<Browser> Browsers { get; set; }

        public BrowserListViewModel(Settings settings)
        {
            Browsers = GetBrowsers.FromSettingsFile(settings, false);
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

            if (!string.IsNullOrEmpty(Rule.Value))
            {
                // TODO
                Debug.WriteLine("Rule: " + Rule.Value + " is store in the browser " + clickedbrowser.Name);
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
