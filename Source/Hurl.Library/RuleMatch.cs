using DotNet.Globbing;
using Hurl.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hurl.Library;

internal class RuleMatch
{
    private static bool CheckMultiple(string link, List<string> rules)
    {
        var value = rules.FirstOrDefault(rule => CheckRule(link, rule), null);

        return value != null;
    }

    public static bool CheckRule(string link, string rule)
    {
        var ruleObj = new Rule(rule);

        Func<string, string, bool> check = ruleObj.Mode switch
        {
            RuleMode.Domain => DomainCheck,
            RuleMode.Regex => RegexCheck,
            RuleMode.Glob => GlobCheck,
            _ => (link, rule) => link.Equals(rule)
        };

        return check(link, ruleObj.RuleContent);
    }

    public static bool DomainCheck(string link, string rule)
    {
        var uri = new Uri(link);
        var domain = uri.Host;

        return domain.Equals(rule);
    }

    private static bool RegexCheck(string link, string rule)
    {
        var r = new Regex(rule);
        return r.IsMatch(link);
    }

    public static bool GlobCheck(string link, string rule)
    {
        var x = Glob.Parse(rule).IsMatch(link);
        return x;
    }
}

