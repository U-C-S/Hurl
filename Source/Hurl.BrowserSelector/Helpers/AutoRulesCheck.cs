using Hurl.Library;
using Hurl.Library.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Hurl.BrowserSelector.Helpers;

internal class AutoRulesCheck(string url, List<Ruleset> rulesets, ObservableCollection<Browser> browsers)
{
    private readonly string _url = url;
    private Browser? _browser;
    private Ruleset? _rule;

    public bool Start()
    {
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
                var x = browsers.ToList().Find(x => x.Name == rules.BrowserName);
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
