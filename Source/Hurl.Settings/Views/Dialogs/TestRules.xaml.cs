using Hurl.Library;
using Hurl.Library.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace Hurl.Settings.Views.Dialogs;

public sealed partial class TestRules : Page
{
    public TestRules()
    {
        this.InitializeComponent();
    }

    private void TestRuleButton_Click(object sender, RoutedEventArgs e)
    {
        var uri = _UriInput.Text;
        var ruleMode = _RuleTypeInput.Text;
        var ruleContent = _RuleInput.Text;

        if (string.IsNullOrWhiteSpace(uri) || string.IsNullOrWhiteSpace(ruleContent))
        {
            PresentOutput("Please fill in all fields - URI, Rule type, Rule", InfoBarSeverity.Error);
            return;
        }
        else if (ruleMode == null)
        {
            PresentOutput("Select a rule type", InfoBarSeverity.Error);
            return;
        }

        var rule = new Rule(ruleContent, ruleMode.ToString());

        if (RuleMatch.CheckRule(uri, rule))
        {
            PresentOutput("Rule matches", InfoBarSeverity.Success);
        }
        else
        {
            PresentOutput("Rule does not match", InfoBarSeverity.Informational);
        }
    }

    private void TestExistingButton_Click(object sender, RoutedEventArgs e)
    {
        var uri = _UriInput.Text;
        var rulesets = State.Settings.GetAutoRoutingRules();

        if (string.IsNullOrWhiteSpace(uri))
        {
            PresentOutput("Please fill in the URI field", InfoBarSeverity.Error);
            return;
        }

        var matchingRuleset = rulesets
            .FirstOrDefault(ruleset => RuleMatch.CheckMultiple(uri, ruleset.Rules), null);

        if (matchingRuleset != null)
        {
            PresentOutput($"Ruleset name: {matchingRuleset?.RulesetName}\nBrowser: {matchingRuleset.BrowserName}", InfoBarSeverity.Success);
        }
        else
        {
            PresentOutput("No ruleset match", InfoBarSeverity.Informational);
        }
    }

    private void PresentOutput(string text, InfoBarSeverity severity)
    {
        _outputCard.IsOpen = true;
        _outputCard.Severity = severity;
        _outputCard.Message = text;
        _outputCard.StartBringIntoView();
    }

    private void CopyRuleButton_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.Clipboard.SetText(_RuleInput.Text);
    }
}
