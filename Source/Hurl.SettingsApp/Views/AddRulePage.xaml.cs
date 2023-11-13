using Hurl.Library.Models;
using Hurl.SettingsApp.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Hurl.SettingsApp.Views;


public sealed partial class AddRulePage : Page
{
    public AddRulePage()
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

    public AutoRoutingRules Generate()
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

        AutoRoutingRules obj = new()
        {
            BrowserName = TargetBrowser.SelectedValue.ToString(),
            AltLaunchIndex = AltLaunchIndex,
            Rules = rules
        };

        return obj;
    }
}

