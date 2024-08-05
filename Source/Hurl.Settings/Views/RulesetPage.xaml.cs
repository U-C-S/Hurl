using Hurl.Library.Models;
using Hurl.Settings.ViewModels;
using Hurl.Settings.Views.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

namespace Hurl.Settings.Views;

public sealed partial class RulesetPage : Page
{
    public RulesetPage()
    {
        InitializeComponent();
    }

    private async void TestButton_Click(object sender, RoutedEventArgs e)
    {
        TestRules TestRulesDialogContent = new();
        ContentDialog testRulesDialog = new()
        {
            XamlRoot = this.XamlRoot,
            Title = "Test rules",
            Content = TestRulesDialogContent,
            CloseButtonText = "Close",
            IsPrimaryButtonEnabled = false,
            DefaultButton = ContentDialogButton.Primary,
        };

        _ = await testRulesDialog.ShowAsync();

    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        NewRulesetDialog NewRulesetDialogContent = new();
        ContentDialog createNewRulesetDialog = new()
        {
            XamlRoot = this.XamlRoot,
            Title = "Create new ruleset",
            Content = NewRulesetDialogContent,
            CloseButtonText = "Cancel",
            PrimaryButtonText = "Create",
            DefaultButton = ContentDialogButton.Primary,
        };

        var result = await createNewRulesetDialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var x = NewRulesetDialogContent.Generate();

            ViewModel.NewRuleset(x);
        }
    }

    private async void ClickEditRuleset(object sender, RoutedEventArgs e)
    {
        var ruleset = (sender as MenuFlyoutItem)!.DataContext as Ruleset;
        var rulesetVm = new StoreRulesetViewModel(ruleset);
        NewRulesetDialog NewRulesetDialogContent = new(rulesetVm);
        ContentDialog createNewRulesetDialog = new()
        {
            XamlRoot = this.XamlRoot,
            Title = $"Edit ruleset {ruleset.RulesetName}",
            Content = NewRulesetDialogContent,
            CloseButtonText = "Cancel",
            PrimaryButtonText = "Save",
            DefaultButton = ContentDialogButton.Primary,
        };

        var result = await createNewRulesetDialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var x = NewRulesetDialogContent.Generate();
            ViewModel.EditRuleset(x);
        }
    }

    private void ClickMoveUpRuleset(object sender, RoutedEventArgs e)
    {
        var Id = (Guid)(sender as MenuFlyoutItem).Tag;

        ViewModel.MoveRulesetUp(Id);
    }
    private void ClickMoveDownRuleset(object sender, RoutedEventArgs e)
    {
        var Id = (Guid)(sender as MenuFlyoutItem).Tag;

        State.Settings.MoveRulesetDown(Id);
    }

    private void ClickDeleteRuleset(object sender, RoutedEventArgs e)
    {
        var Id = (Guid)(sender as MenuFlyoutItem).Tag;

        State.Settings.DeleteRuleset(Id);
    }
}

