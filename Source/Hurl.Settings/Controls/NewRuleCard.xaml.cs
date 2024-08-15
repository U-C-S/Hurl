using Hurl.Library.Models;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Hurl.Settings.Controls;

public sealed partial class NewRuleCard : UserControl
{
    public NewRuleCard()
    {
        InitializeComponent();

        RuleTypeControl.ItemsSource = Enum.GetValues(typeof(RuleMode));
    }

    public NewRuleCard(Rule rule)
    {
        InitializeComponent();
        var ruleModes = Enum.GetValues(typeof(RuleMode));

        RuleTypeControl.ItemsSource = ruleModes;
        RuleTypeControl.SelectedIndex = Array.IndexOf(ruleModes, rule.Mode); ;
        RuleValueControl.Text = rule.RuleContent;
    }

    public Rule? ConstructRule()
    {
        var ruleType = RuleTypeControl.SelectedValue.ToString();
        var ruleValue = RuleValueControl.Text;

        if (!string.IsNullOrWhiteSpace(ruleValue))
            return new Rule(ruleValue, ruleType);
        return null;
    }

    private void DeleteButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (Parent is StackPanel stackPanel)
            stackPanel.Children.Remove(this);
    }
}

