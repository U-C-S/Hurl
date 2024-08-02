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

    public string ConstructRule()
    {
        var ruleType = RuleTypeControl.SelectedValue.ToString();
        var ruleValue = RuleValueControl.Text;

        return new Rule(ruleValue, ruleType).ToString();
    }
}

