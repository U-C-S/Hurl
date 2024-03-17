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
        var ruleMode = _RuleTypeInput.SelectedValue;
        var ruleContent = _RuleInput.Text;

        var rule = new Rule(ruleContent, ruleMode.ToString());

        if (RuleMatch.CheckRule(uri, rule))
        {
            PresentOutput(new TextBlock
            {
                Text = "Rule Match",
                FontSize = 16,
                Foreground = Brushes.Green,
                HorizontalAlignment = HorizontalAlignment.Center
            });
        }
        else
        {
            PresentOutput(new TextBlock
            {
                Text = "Rule does not Match",
                FontSize = 16,
                Foreground = Brushes.Red,
                HorizontalAlignment = HorizontalAlignment.Center
            });
        }
    }

    private void TestExistingButton_Click(object sender, RoutedEventArgs e)
    {
        var uri = _UriInput.Text;
        var rulesets = SettingsState.Rulesets;

        var matchingRuleset = rulesets
            .FirstOrDefault(ruleset => RuleMatch.CheckMultiple(uri, ruleset.Rules), null);

        if (matchingRuleset != null)
        {
            PresentOutput(new TextBlock
            {
                Text = $"Ruleset Match: {matchingRuleset.BrowserName}\nRuleset Index: {matchingRuleset.Id}",
                FontSize = 16,
                Foreground = Brushes.Green,
                HorizontalAlignment = HorizontalAlignment.Center
            });
        }
        else
        {
            PresentOutput(new TextBlock
            {
                Text = "No Ruleset Match",
                FontSize = 16,
                Foreground = Brushes.Red,
                HorizontalAlignment = HorizontalAlignment.Center
            });
        }
    }

    private void PresentOutput(UIElement elem)
    {
        _outputCard.Visibility = Visibility.Visible;
        _outputCard.Children.Clear();
        _outputCard.Children.Add(elem);
    }

    private void CopyRuleButton_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(_RuleInput.Text);
    }
}
