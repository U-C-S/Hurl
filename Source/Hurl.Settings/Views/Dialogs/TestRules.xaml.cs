using Hurl.Library;
using Hurl.Library.Models;
using Hurl.Settings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace Hurl.Settings.Views.Dialogs;

public sealed partial class TestRules : Page
{
    public RulesetPageViewModel ViewModel { get; }

    public TestRules()
    {
        this.InitializeComponent();
        ViewModel = App.AppHost.Services.GetRequiredService<RulesetPageViewModel>();
    }

    private void TestRuleButton_Click(object sender, RoutedEventArgs e)
    {
        var uri = _UriInput.Text;
        var ruleMode = _RuleTypeInput.SelectedValue;
        var ruleContent = _RuleInput.Text;

        if (string.IsNullOrWhiteSpace(uri)
            || string.IsNullOrWhiteSpace(ruleContent)
            || ruleMode is null)
        {
            PresentOutput("Please fill in all fields - URI, Rule Type, Rule", InfoBarSeverity.Error);
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
        var rulesets = ViewModel.Rulesets;

        if (string.IsNullOrWhiteSpace(uri))
        {
            PresentOutput("Please fill in the URI field", InfoBarSeverity.Error);
            return;
        }

        var matchingRuleset = rulesets
            .FirstOrDefault(ruleset => RuleMatch.CheckMultiple(uri, ruleset.Rules), null);

        if (matchingRuleset != null)
        {
            PresentOutput($"Ruleset match: {matchingRuleset.BrowserName}\nRuleset name: {matchingRuleset?.RulesetName}", InfoBarSeverity.Success);
        }
        else
        {
            PresentOutput("No Ruleset match", InfoBarSeverity.Informational);
        }
    }

    private void PresentOutput(string text, InfoBarSeverity severity)
    {
        var textBlock = new InfoBar
        {
            Message = text,
            IsOpen = true,
            Severity = severity,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        _outputCard.Visibility = Visibility.Visible;
        _outputCard.Children.Clear();
        _outputCard.Children.Add(textBlock);
    }

    private void CopyRuleButton_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.Clipboard.SetText(_RuleInput.Text);
    }
}
