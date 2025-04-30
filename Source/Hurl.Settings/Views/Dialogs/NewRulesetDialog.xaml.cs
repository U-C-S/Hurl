using Hurl.Library.Models;
using Hurl.Settings.Controls;
using Hurl.Settings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Hurl.Settings.Views.Dialogs;

public sealed partial class NewRulesetDialog : Page
{
    public StoreRulesetViewModel viewModel { get; }

    public NewRulesetDialog()
    {
        this.InitializeComponent();
        viewModel = App.AppHost.Services.GetRequiredService<StoreRulesetViewModel>();
    }

    public NewRulesetDialog(StoreRulesetViewModel vm)
    {
        this.InitializeComponent();

        viewModel = vm;
        vm.Rules.ForEach(rule => RulesStack.Children.Add(new NewRuleCard(rule)));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        RulesStack.Children.Add(new NewRuleCard());
    }

    private void DeleteRule(int index)
    {
        RulesStack.Children.RemoveAt(index);
    }

    public Ruleset Generate()
    {
        var rulesControls = RulesStack.Children.Cast<NewRuleCard>();
        var rules = new List<Rule>();

        foreach (NewRuleCard ruleControl in rulesControls)
        {
            var rule = ruleControl.ConstructRule();
            if (rule != null)
                rules.Add(rule);
        }
        viewModel.Rules = rules;
        return viewModel.ToRuleSet();
    }

    private void TargetBrowser_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // At this point, AltLaunches can be of List<string> type
        TargetAltLaunch.ItemsSource = viewModel.AltLaunches;
    }
}
