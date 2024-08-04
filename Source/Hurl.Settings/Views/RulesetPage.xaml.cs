using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Hurl.Settings.ViewModels;
using Hurl.Settings.Views.Dialogs;
using Hurl.Library.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hurl.Settings.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RulesetPage : Page
    {
        public RulesetViewModel ViewModel { get; set; }

        public RulesetPage()
        {
            ViewModel = new();
            DataContext = ViewModel;

            this.InitializeComponent();
        }

        private async void TestButton_Click(object sender, RoutedEventArgs e)
        {
            TestRules TestRulesDialogContent = new();
            ContentDialog testRulesDialog = new()
            {
                XamlRoot = this.XamlRoot,
                Title = "Test rules",
                Content = TestRulesDialogContent,
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Test",
                DefaultButton = ContentDialogButton.Primary,
            };

            var result = await testRulesDialog.ShowAsync();
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

                State.Settings.AddRuleset(x);
            }
        }

        private async void ClickEditRuleset(object sender, RoutedEventArgs e)
        {
            var ruleset = (sender as MenuFlyoutItem)!.DataContext as Ruleset;
            var ViewModel = new StoreRulesetViewModel(ruleset);
            NewRulesetDialog NewRulesetDialogContent = new(ViewModel);
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

                State.Settings.AddRuleset(x);
            }
        }

        private void ClickMoveUpRuleset(object sender, RoutedEventArgs e)
        {
            int Id = (int)(sender as MenuFlyoutItem).Tag;

            ViewModel.MoveRulesetUp(Id);
        }
        private void ClickMoveDownRuleset(object sender, RoutedEventArgs e)
        {
            int Id = (int)(sender as MenuFlyoutItem).Tag;

            State.Settings.MoveRulesetDown(Id);
        }

        private void ClickDeleteRuleset(object sender, RoutedEventArgs e)
        {
            int Id = (int)(sender as MenuFlyoutItem).Tag;

            State.Settings.DeleteRuleset(Id);
        }
    }
}
