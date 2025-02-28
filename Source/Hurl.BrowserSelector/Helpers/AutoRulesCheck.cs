using Hurl.Library;
using Hurl.Library.Models;
using System.Diagnostics;

namespace Hurl.BrowserSelector.Helpers
{
    internal class AutoRulesCheck(string url)
    {
        private readonly string _url = url;
        private Browser? _browser;
        private Ruleset? _rule;

        public bool Start()
        {
            var rulesets = State.Settings.Rulesets;

#if DEBUG
            Stopwatch sw = new();
            sw.Start();
#endif
            foreach (var rules in rulesets)
            {
                var isHurl = rules.BrowserName == "_Hurl";
                if (isHurl && RuleMatch.CheckMultiple(_url, rules.Rules))
                {
                    return false;
                }
                else
                {
                    var x = State.Settings.Browsers.Find(x => x.Name == rules.BrowserName);
                    if (x != null)
                    {
                        if (RuleMatch.CheckMultiple(_url, rules.Rules))
                        {
                            _browser = x;
                            _rule = rules;

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

        public void LaunchIfMatch()
        {
            if (_browser != null && _rule != null)
            {
                UriLauncher.ResolveAutomatically(_url, _browser, _rule.AltLaunchIndex);
            }
        }
    }
}
