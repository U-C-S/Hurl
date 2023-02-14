using DotNet.Globbing;
using Hurl.Library.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hurl.BrowserSelector.Helpers
{
    class AutoRulesCheck
    {
        static bool Check(string link, string[] rules)
        {
            //if (link.StartsWith("http"))
            //{
            //    int index = link.IndexOf("://");
            //    if (index != -1)
            //    {
            //        link = link.Substring(index + 3);
            //    }
            //}

            //var uri = new Uri(link);
            //link = uri.Host;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var value = rules.FirstOrDefault(rule =>
            {
                if (rule.StartsWith("r$")) //regex mode
                {
                    var r = new Regex(rule.Substring(2));
                    return r.IsMatch(link);
                }
                else if (rule.StartsWith("g$")) //glob mode
                {
                    return Glob.Parse(rule.Substring(2)).IsMatch(link);
                }
                else
                {
                    return link.Equals(rule);
                }
            }, null);
            sw.Stop();
            Debug.WriteLine(sw.Elapsed.TotalSeconds);
            return value != null;
        }

        /*
        public static bool CheckAndRun(string link, string[] rules)
        {
            var linkPattern = Check(link, rules);
            if (linkPattern != null)
            {
                Process.Start(linkPattern.Browser.ExePath, link);
                return true;
            }
            return false;
        }
        */

        public static bool CheckAllBrowserRules(string link, IEnumerable<Browser> browsers)
        {
            foreach (var browser in browsers)
            {
                if (browser.Rules != null)
                {
                    if (Check(link, browser.Rules))
                    {
                        Process.Start(browser.ExePath, link);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
