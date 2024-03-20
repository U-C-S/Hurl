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
        string uri = _UriInput.Text;
        string ruleMode = _RuleTypeInput.Text;
        string ruleContent = _RuleInput.Text;
        var rulesets = SettingsState.Rulesets;

        if (UseCustomRule.IsChecked == true)
        {
            var rule = new Rule(ruleContent, ruleMode);

            if (RuleMatch.CheckRule(uri, rule))
            {
                //PresentOutput("Rule matches", Brushes.Green);
                _outputCard.IsOpen = true;
                _outputCard.Severity = Wpf.Ui.Controls.InfoBarSeverity.Success;
                _outputCard.Title = "Match";
                _outputCard.Message = string.Empty;
            }
            else
            {
                //PresentOutput("Rule does not match", Brushes.Red);
                _outputCard.IsOpen = true;
                _outputCard.Severity = Wpf.Ui.Controls.InfoBarSeverity.Error;
                _outputCard.Title = "No match";
                _outputCard.Message = string.Empty;
            }
        }
        else
        {
            // ---
            var matchingRuleset = rulesets
                .FirstOrDefault(ruleset => RuleMatch.CheckMultiple(uri, ruleset.Rules), null);

            if (matchingRuleset != null)
            {
                //PresentOutput($"Ruleset Match: {matchingRuleset.BrowserName}\nRuleset Name: {matchingRuleset?.Name}", Brushes.Green);
                _outputCard.IsOpen = true;
                _outputCard.Severity = Wpf.Ui.Controls.InfoBarSeverity.Success;
                _outputCard.Title = "Match";
                _outputCard.Message = $"Browser: {matchingRuleset.BrowserName}\nRuleset Name: {matchingRuleset?.Name}";
            }
            else
            {
                //PresentOutput("No Ruleset Match", Brushes.Red);
                _outputCard.IsOpen = true;
                _outputCard.Severity = Wpf.Ui.Controls.InfoBarSeverity.Error;
                _outputCard.Title = "No rule match";
                _outputCard.Message = string.Empty;
            }
        }
    }
}
