using Hurl.BrowserSelector.Models;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hurl.BrowserSelector.Helpers
{
    class AutoRulesCheck
    {
        static LinkPattern Check(string link, LinkPattern[] rules)
        {
            var value = rules.FirstOrDefault(rule => { return Regex.IsMatch(link, rule.Pattern); }, null);
            return value;
        }

        public static bool CheckAndRun(string link, LinkPattern[] rules)
        {
            var linkPattern = Check(link, rules);
            if (linkPattern != null)
            {
                Process.Start(linkPattern.Browser.ExePath, link);
                return true;
            }
            return false;
        }
    }
}
