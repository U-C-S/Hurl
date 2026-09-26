using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models;

public partial class Ruleset : ObservableObject
{
    [ObservableProperty]
    public partial Guid Id { get; set; } = Guid.NewGuid();

    [ObservableProperty]
    public partial List<string> Rules { get; set; } = [];

    [ObservableProperty]
    public partial string RulesetName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial Guid BrowserId { get; set; }

    [ObservableProperty]
    [field: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public partial Guid? AlternateLaunchId { get; set; }
}

public enum RuleMode
{
    Domain,
    Regex,
    String,
}

public class Rule
{
    public Rule(string RuleContent, string? Mode)
    {
        this.RuleContent = RuleContent;
        this.Mode = Mode switch
        {
            "Domain" => RuleMode.Domain,
            "Regex" => RuleMode.Regex,
            _ => RuleMode.String
        };
    }

    public Rule(string storedRule)
    {
        if (storedRule.Length >= 2 
            && storedRule[1] == '$' 
            && storedRule[0] is 'd' or 'r' or 's')
        {
            RuleContent = storedRule[2..];
            Mode = storedRule[0] switch
            {
                'd' => RuleMode.Domain,
                'r' => RuleMode.Regex,
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
            RuleMode.String => $"s${RuleContent}",
            RuleMode.Regex => $"r${RuleContent}",
            _ => throw new NotImplementedException()
        };

        return RuleString;
    }
}
