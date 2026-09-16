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
        if (!Uri.TryCreate(link, UriKind.Absolute, out var uri))
        {
            return false;
        }

        var domain = uri.Host;

        // A rule of the form "*.example.com" matches "example.com" itself
        // as well as any of its subdomains (e.g. "docs.example.com").
        if (rule.StartsWith("*.", StringComparison.Ordinal))
        {
            var baseDomain = rule[2..];

            return domain.Equals(baseDomain, StringComparison.OrdinalIgnoreCase)
                || domain.EndsWith("." + baseDomain, StringComparison.OrdinalIgnoreCase);
        }

        return domain.Equals(rule, StringComparison.OrdinalIgnoreCase);
    }

    private static bool RegexCheck(string link, string rule)
    {
        var r = new Regex(rule);
        return r.IsMatch(link);
    }
}

