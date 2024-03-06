using Hurl.Library.Models;
using Hurl.RulesetManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hurl.RulesetManager.Windows;

public partial class EditRuleset
{
    //public Ruleset? Ruleset { get; set; }

    public EditRuleset(EditRulesetViewModel? vm)
    {
        InitializeComponent();

        DataContext = vm;
    }

    private void Refresh()
    {

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var selectedBrowser = TargetBrowser.SelectedValue;
        var rules = "RuleInput.Text";
        if (selectedBrowser == null || string.IsNullOrEmpty(rules))
        {
            WarnText.Visibility = Visibility.Visible;
            WarnText.Height = 20;
            return;
        };

        var rulesList = new List<string>();

        foreach (var rule in rules.Split('|'))
        {
            rulesList.Add(rule.Trim());
        };

        //SettingsGlobal.AddBrowserRule(rulesList, TargetBrowser.SelectedValue.ToString());

        this.Close();
    }

    private void Save()
    {
        var id = ((Ruleset)DataContext).Id;



    }
}

