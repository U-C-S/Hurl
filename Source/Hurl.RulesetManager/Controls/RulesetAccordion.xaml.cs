using Hurl.Library.Models;
using Hurl.RulesetManager.ViewModels;
using Hurl.RulesetManager.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.RulesetManager.Controls;

public partial class RulesetAccordion : UserControl
{
    private readonly int _index;

    public RulesetAccordion(int index, Ruleset ruleset)
    {
        _index = index;
        InitializeComponent();

        DataContext = ruleset;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var ruleset = ((Ruleset)DataContext);

        var ViewModel = new EditRulesetViewModel(_index, ruleset);
        var window = new EditRuleset(ViewModel, UpdateRuleset) { Owner = Window.GetWindow(this) };
        window.ShowDialog();
    }

    public void UpdateRuleset(EditRulesetViewModel vm)
    {
        var ruleset = vm.ToRuleSet();
        DataContext = ruleset;

        SettingsState.Rulesets = SettingsState.Rulesets
            .Select((x, i) => i == _index ? ruleset : x)
            .ToList();
        SettingsState.Update();
    }
}

