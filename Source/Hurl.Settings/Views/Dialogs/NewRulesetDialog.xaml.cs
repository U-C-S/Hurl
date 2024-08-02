using Hurl.Library.Models;
using Hurl.Settings.Controls;
using Hurl.Settings.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Hurl.Settings.Views.Dialogs;

public sealed partial class NewRulesetDialog : Page
{
    public StoreRulesetViewModel viewModel { get; set; }

    public NewRulesetDialog()
    {
        this.InitializeComponent();

        viewModel = new();
    }

    public NewRulesetDialog(StoreRulesetViewModel vm)
    {
        this.InitializeComponent();

        viewModel = vm;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        RulesStack.Children.Add(new NewRuleCard());
    }

    public Ruleset Generate()
    {
        var rulesControls = RulesStack.Children.Cast<NewRuleCard>();
        List<string> rules = new();

        foreach (NewRuleCard ruleControl in rulesControls)
        {
            string rule = ruleControl.ConstructRule();
            if (!string.IsNullOrEmpty(rule))
            {
                rules.Add(rule);
            }
        }

        return viewModel.ToRuleSet();
    }

    private void TargetBrowser_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // At this point, AltLaunches can be of List<string> type
        TargetAltLaunch.ItemsSource = viewModel.AltLaunches;
    }
}
