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
using Hurl.Library.Models;
using Hurl.Settings.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hurl.Settings.Views.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewRulesetDialog : Page
    {
        public NewRulesetDialog()
        {
            this.InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RulesStack.Children.Add(new NewRuleCard());
        }

        List<string> AllBrowsers
        {
            get
            {
                return State.Settings.GetBrowserNames();
            }
        }

        List<string> AltLaunches
        {
            get
            {
                object selectedBrowser = TargetBrowser.SelectedValue;
                if (selectedBrowser == null)
                {
                    return new List<string>();
                }

                return State.Settings.GetAltLaunchesNamesForBrowser(selectedBrowser.ToString());
            }
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

            int AltLaunchIndex = TargetAltLaunch.SelectedIndex == -1 ? -1 : AltLaunches.IndexOf(TargetAltLaunch.SelectedValue.ToString());

            Ruleset obj = new()
            {
                BrowserName = TargetBrowser.SelectedValue.ToString(),
                AltLaunchIndex = AltLaunchIndex,
                Rules = rules
            };

            return obj;
        }
    }
}
