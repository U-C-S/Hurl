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
        _TitleBar.Title = $"Edit Ruleset - {vm?.Name}";
    }

    public EditRuleset(Action<EditRulesetViewModel> createSuccessCallback)
    {
        InitializeComponent();
        DataContext = new EditRulesetViewModel();
        _successCallback = createSuccessCallback;
        _TitleBar.Title = "Create new ruleset";
    }

    private readonly Action<EditRulesetViewModel> _successCallback;

    private void Refresh()
    {
        var vm = (EditRulesetViewModel)DataContext;

        _RulesListControl.ItemsSource = vm.Rules;
        _RulesListControl.Items.Refresh();
    }

    private void TargetBrowser_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int selectedIndex = ((ComboBox)sender).SelectedIndex;
        var vm = (EditRulesetViewModel)DataContext;

        vm.SelectedBrowser = selectedIndex;
        TargetAltLaunch.ItemsSource = vm.AltLaunches;
    }

    private void RuleAddButton_Click(object sender, RoutedEventArgs e)
    {
        string rule = _Rule.Text;
        string mode = _RuleInputType.Text;

        if (string.IsNullOrWhiteSpace(rule) || string.IsNullOrWhiteSpace(mode))
        {
            return;
        }

        var vm = (EditRulesetViewModel)DataContext;
        var ruleObj = new Rule(rule, mode);
        vm.Rules.Add(ruleObj);

        Refresh();

        _Rule.Text = string.Empty;
        _Rule.Focus();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        _successCallback(DataContext as EditRulesetViewModel);
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void RuleRemoveButton_Click(object sender, RoutedEventArgs e)
    {
        var rule = (Rule)((Button)sender).DataContext;
        var vm = (EditRulesetViewModel)DataContext;
        vm.Rules.Remove(rule);

        Refresh();
    }
}

