using Hurl.Library;
using Hurl.Library.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hurl.RulesetManager.Windows;

public partial class RuleTestingWindow
{
    public RuleTestingWindow()
    {
        InitializeComponent();
    }

    private void TestRuleButton_Click(object sender, RoutedEventArgs e)
    {
        var uri = _UriInput.Text;
        var ruleMode = _RuleTypeInput.Text;
        var ruleContent = _RuleInput.Text;

        if (string.IsNullOrWhiteSpace(uri) || string.IsNullOrWhiteSpace(ruleContent))
        {
            PresentOutput("Please fill in all fields - URI, Rule Type, Rule", Brushes.Red);
            return;
        }
        else if (ruleMode == null)
        {
            PresentOutput("Select a rule type", Brushes.Red);
            return;
        }

        var rule = new Rule(ruleContent, ruleMode.ToString());

        if (RuleMatch.CheckRule(uri, rule))
        {
            PresentOutput("Rule matches", Brushes.Green);
        }
        else
        {
            PresentOutput("Rule does not match", Brushes.Red);
        }
    }

    private void TestExistingButton_Click(object sender, RoutedEventArgs e)
    {
        var uri = _UriInput.Text;
        var rulesets = SettingsState.Rulesets;

        if(string.IsNullOrWhiteSpace(uri))
        {
            PresentOutput("Please fill in the URI field", Brushes.Red);
            return;
        }

        var matchingRuleset = rulesets
            .FirstOrDefault(ruleset => RuleMatch.CheckMultiple(uri, ruleset.Rules), null);

        if (matchingRuleset != null)
        {
            PresentOutput($"Ruleset Match: {matchingRuleset.BrowserName}", Brushes.Green);
        }
        else
        {
            PresentOutput("No Ruleset Match", Brushes.Red);
        }
    }

    private void PresentOutput(string text, Brush foregroudColor)
    {
        var textBlock = new TextBlock
        {
            Text = text,
            FontSize = 16,
            Foreground = foregroudColor,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        _outputCard.Visibility = Visibility.Visible;
        _outputCard.Children.Clear();
        _outputCard.Children.Add(textBlock);
    }

    private void CopyRuleButton_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(_RuleInput.Text);
    }
}
