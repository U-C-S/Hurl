using Hurl.Library.Models;
using Hurl.RulesetManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.RulesetManager.Windows;

public partial class EditRuleset
{
    public EditRuleset(EditRulesetViewModel? vm, Action<EditRulesetViewModel> successCallback)
    {
        InitializeComponent();
        DataContext = vm;
        _successCallback = successCallback;
    }

    public EditRuleset(Action<EditRulesetViewModel> createSuccessCallback)
    {
        InitializeComponent();
        DataContext = new EditRulesetViewModel();
        _successCallback = createSuccessCallback;
    }

    private readonly Action<EditRulesetViewModel> _successCallback;

    private void Refresh()
    {
        var vm = (EditRulesetViewModel)DataContext;

        _RulesListControl.ItemsSource = vm.Rules;
        _RulesListControl.Items.Refresh();
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

    private void TargetBrowser_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedIndex = ((ComboBox)sender).SelectedIndex;
        var vm = (EditRulesetViewModel)DataContext;

        vm.SelectedBrowser = selectedIndex;
        TargetAltLaunch.ItemsSource = vm.AltLaunches;
    }

    private void RuleAddButton_Click(object sender, RoutedEventArgs e)
    {
        var rule = _Rule.Text;
        var mode = _RuleInputType.Text;
        var vm = (EditRulesetViewModel)DataContext;

        var ruleObj = new Rule(rule, mode);

        vm.Rules.Add(ruleObj);
        Refresh();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        _successCallback(DataContext as EditRulesetViewModel);
        this.Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        this.Close();
    }
}

