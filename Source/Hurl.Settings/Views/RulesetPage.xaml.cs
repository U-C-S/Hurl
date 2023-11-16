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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hurl.Settings.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RulesetPage : Page
    {
        public RulesetPage()
        {
            this.InitializeComponent();
        }

        public RulesetViewModel ViewModel => new();

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
            createNewRulesetDialog.Content = new NewRulesetDialog();

            var result = await createNewRulesetDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var x = ((NewRulesetDialog)createNewRulesetDialog.Content).Generate();

                State.Settings.AddRuleset(x);
            }
        }
    }
}
