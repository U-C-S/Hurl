using DotNet.Globbing;
using Hurl.BrowserSelector.Globals;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hurl.BrowserSelector.Helpers
{
    internal class AutoRulesCheck
    {
        private static bool Check(string link, List<string> rules)
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
            Stopwatch sw = new();
            sw.Start();
            var value = rules.FirstOrDefault(rule =>
            {
                // regex mode
                // ex: "r$.*\\.google\\.com"
                if (rule.StartsWith("r$"))
                {
                    var r = new Regex(rule.Substring(2));
                    return r.IsMatch(link);
                }
                // glob mode
                // ex: "g$*.google.com"
                else if (rule.StartsWith("g$"))
                {
                    var globpattern = rule.Substring(2);
                    if (link.StartsWith("http"))
                    {
                        int index = link.IndexOf("://");
                        if (index != -1)
                        {
                            link = link.Substring(index + 3);
                        }
                    }
                    var x = Glob.Parse(globpattern).IsMatch(link);
                    return x;
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

        public static bool Start(string link)
        {
            var settings = SettingsGlobal.Value;

            foreach (var rules in settings.AutoRoutingRules)
            {
                var isHurl = rules.BrowserName == "_Hurl";
                if (isHurl && Check(link, rules.Rules))
                {
                    return false;
                }
                else
                {
                    var x = settings.Browsers.Find(x => x.Name == rules.BrowserName);
                    if (x != null)
                    {
                        if (Check(link, rules.Rules))
                        {
                            Process.Start(x.ExePath, link);

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
