using Hurl.Library.Models;
using Hurl.Settings.ViewModels;
using Hurl.Settings.Views.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

namespace Hurl.Settings.Views;

public sealed partial class RulesetPage : Page
{
    public RulesetPageViewModel ViewModel { get; }

    public RulesetPage()
    {
        InitializeComponent();
        ViewModel = App.AppHost.Services.GetRequiredService<RulesetPageViewModel>();
        if (ViewModel.Rulesets.Count == 0)
            _RulesetListControl.IsExpanded = false;
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

    private async void CreateButton_Click(object sender, RoutedEventArgs e)
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
            var warn = Validate(x);

            if (!string.IsNullOrEmpty(warn))
            {
                RulesetWarnFlyout.Subtitle = warn;
                RulesetWarnFlyout.Target = sender as Button;
                RulesetWarnFlyout.IsOpen = true;
            }

            ViewModel.NewRuleset(x);
        }
    }

    private async void ClickEditRuleset(object sender, RoutedEventArgs e)
    {
        var id = (Guid)(sender as MenuFlyoutItem)!.Tag;
        var rulesetVm = ActivatorUtilities.CreateInstance<StoreRulesetViewModel>(App.AppHost.Services, id);
        NewRulesetDialog NewRulesetDialogContent = new(rulesetVm);
        ContentDialog createNewRulesetDialog = new()
        {
            XamlRoot = this.XamlRoot,
            Title = $"Edit Ruleset",
            Content = NewRulesetDialogContent,
            CloseButtonText = "Cancel",
            PrimaryButtonText = "Save",
            DefaultButton = ContentDialogButton.Primary,
        };

        var result = await createNewRulesetDialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var x = NewRulesetDialogContent.Generate();

            var warn = Validate(x);

            if (!string.IsNullOrEmpty(warn))
            {
                RulesetWarnFlyout.Subtitle = warn;
                RulesetWarnFlyout.IsOpen = true;
            }

            ViewModel.EditRuleset(x);
        }
    }

    private string? Validate(Ruleset rs)
    {
        if (string.IsNullOrWhiteSpace(rs.BrowserName))
        {
            return "Missing Browser. This prevents configured rules from triggering";
        }
        else if (rs.Rules?.Count == 0)
        {
            return "No rules configured";
        }
        else if (string.IsNullOrWhiteSpace(rs.RulesetName))
        {
            return "Missing Ruleset Name";
        }
        else return null;
    }

    private async void ViewRuleset(object sender, RoutedEventArgs e)
    {
        var id = (Guid)(sender as Button)!.Tag;
        var ruleset = ViewModel.Rulesets.Where(x => x.Id == id).First();
        ViewRulesDialog ViewRulesDialogContent = new(ruleset);
        ContentDialog viewRulesDialog = new()
        {
            XamlRoot = this.XamlRoot,
            Title = $"Viewing Ruleset - {ruleset.RulesetName}",
            Content = ViewRulesDialogContent,
            CloseButtonText = "Close",
            DefaultButton = ContentDialogButton.Close,
        };

        _ = await viewRulesDialog.ShowAsync();
    }

    private void ClickMoveUpRuleset(object sender, RoutedEventArgs e)
    {
        var Id = (Guid)(sender as MenuFlyoutItem)!.Tag;

        ViewModel.MoveRulesetUp(Id);
    }
    private void ClickMoveDownRuleset(object sender, RoutedEventArgs e)
    {
        var Id = (Guid)(sender as MenuFlyoutItem)!.Tag;

        ViewModel.MoveRulesetDown(Id);
    }

    private void ClickDeleteRuleset(object sender, RoutedEventArgs e)
    {
        var Id = (Guid)(sender as MenuFlyoutItem)!.Tag;

        ViewModel.DeleteRuleset(Id);
    }
}

