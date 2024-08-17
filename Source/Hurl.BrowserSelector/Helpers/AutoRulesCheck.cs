using Hurl.BrowserSelector.State;
using Hurl.Library;
using System.Diagnostics;

namespace Hurl.BrowserSelector.Helpers
{
    internal class AutoRulesCheck
    {
        public static bool Start(string link)
        {
            var rulesets = Settings.Rulesets;

#if DEBUG
            Stopwatch sw = new();
            sw.Start();
#endif
            foreach (var rules in rulesets)
            {
                var isHurl = rules.BrowserName == "_Hurl";
                if (isHurl && RuleMatch.CheckMultiple(link, rules.Rules))
                {
                    return false;
                }
                else
                {
                    var x = Settings.Browsers.Find(x => x.Name == rules.BrowserName);
                    if (x != null)
                    {
                        if (RuleMatch.CheckMultiple(link, rules.Rules))
                        {
                            UriLauncher.ResolveAutomatically(link, x, rules.AltLaunchIndex);

                            return true;
                        }
                    }
                }
            }
#if DEBUG
            sw.Stop();
            Debug.WriteLine(sw.Elapsed.TotalSeconds);
#endif
            return false;
        }
    }
}
