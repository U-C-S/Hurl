// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Hurl.SettingsApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hurl.SettingsApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RulesPage : Page
    {
        public RulesPage()
        {
            this.InitializeComponent();
        }

        public RulesViewModel ViewModel => new();

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog createNewRulesetDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Create new ruleset",
                Content = "Enter a name for your new ruleset:",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Create",
                DefaultButton = ContentDialogButton.Primary,
            };
            createNewRulesetDialog.Content = new AddRulePage();

            var result = await createNewRulesetDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var x = ((AddRulePage)createNewRulesetDialog.Content).Generate();

                State.Settings.AddRuleset(x);
            }
        }
    }
}
