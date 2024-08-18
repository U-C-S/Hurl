using DotNet.Globbing;
using Hurl.Library.Models;
using System.Text.RegularExpressions;

namespace Hurl.Library;

public class RuleMatch
{
    public static bool CheckMultiple(string link, List<string> rules)
    {
        var value = rules.FirstOrDefault(rule => CheckRule(link, rule), null);

        return value != null;
    }

    public static bool CheckRule(string link, string? rule)
    {
        if (rule == null)
        {
            return false;
        }
        var ruleObj = new Rule(rule);
        return CheckRule(link, ruleObj);
    }

    public static bool CheckRule(string link, Rule rule)
    {
        try
        {
            Func<string, string, bool> check = rule.Mode switch
            {
                RuleMode.Domain => DomainCheck,
                RuleMode.Regex => RegexCheck,
                RuleMode.Glob => GlobCheck,
                _ => (link, rule) => link.Equals(rule)
            };

            return check(link, rule.RuleContent);

        }
        catch (Exception)
        {
            return false;
        }
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

