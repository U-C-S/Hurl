using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models;

public class Ruleset
{
    // assign a random number to this
    public int Id { get; set; } = new Random().Next(10000);

    public List<string> Rules { get; set; }

    public string BrowserName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int AltLaunchIndex { get; set; }
}


public enum RuleMode
{
    Domain,
    String,
    Regex,
    Glob
}


public class Rule
{
    public Rule(string RuleContent, string Mode)
    {
        this.RuleContent = RuleContent;
        this.Mode = Mode switch
        {
            "Domain" => RuleMode.Domain,
            "Regex" => RuleMode.Regex,
            "Glob" => RuleMode.Glob,
            _ => RuleMode.String
        };
    }

    public Rule(string storedRule)
    {
        var split = storedRule.Split('$', 2);
        var modeLetter = split[0];
        var content = split[1];

        this.RuleContent = content;
        this.Mode = modeLetter switch
        {
            "d" => RuleMode.Domain,
            "r" => RuleMode.Regex,
            "g" => RuleMode.Glob,
            _ => RuleMode.String
        };
    }

    public string RuleContent { get; set; }

    public RuleMode Mode { get; set; }

    public override string ToString()
    {
        string RuleString = Mode switch
        {
            RuleMode.Domain => $"d${RuleContent}",
            RuleMode.String => $"{RuleContent}",
            RuleMode.Regex => $"r${RuleContent}",
            RuleMode.Glob => $"g${RuleContent}",
            _ => throw new NotImplementedException()
        };

        return RuleString;
    }
}