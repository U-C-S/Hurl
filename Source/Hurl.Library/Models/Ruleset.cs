using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models;

public partial class Ruleset: ObservableObject
{
    [ObservableProperty]
    public Guid id = Guid.NewGuid();

    [ObservableProperty]
    public List<string> rules = [];

    [ObservableProperty]
    public string rulesetName = string.Empty;

    [ObservableProperty]
    public string browserName = string.Empty;

    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? altLaunchIndex;
}

public enum RuleMode
{
    Domain,
    Glob,
    Regex,
    String,
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
        if (storedRule.Contains('$'))
        {
            var split = storedRule.Split('$', 2);
            var modeLetter = split[0];
            var content = split[1];

            RuleContent = content;
            Mode = modeLetter switch
            {
                "d" => RuleMode.Domain,
                "r" => RuleMode.Regex,
                "g" => RuleMode.Glob,
                _ => RuleMode.String
            };
        }
        else
        {
            RuleContent = storedRule;
            Mode = RuleMode.String;
        }
    }

    public string RuleContent { get; set; }

    public RuleMode Mode { get; set; }

    public override string ToString()
    {
        string RuleString = Mode switch
        {
            RuleMode.Domain => $"d${RuleContent}",
            RuleMode.String => $"{RuleContent}", // use this as default instead
            RuleMode.Regex => $"r${RuleContent}",
            RuleMode.Glob => $"g${RuleContent}",
            _ => throw new NotImplementedException()
        };

        return RuleString;
    }
}