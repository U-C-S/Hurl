using Hurl.Library.Models;
using Hurl.RulesetManager.ViewModels;
using Hurl.RulesetManager.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.RulesetManager.Controls;

public partial class RulesetAccordion : UserControl
{
    public RulesetAccordion(Ruleset ruleset)
    {
        InitializeComponent();

        DataContext = ruleset;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var ruleset = ((Ruleset)DataContext);

        var ViewModel = new EditRulesetViewModel(ruleset);
        var window = new EditRuleset(ViewModel, UpdateRuleset) { Owner = Window.GetWindow(this) };
        window.ShowDialog();
    }

    public void UpdateRuleset(EditRulesetViewModel vm)
    {
        var ruleset = vm.ToRuleSet();
        DataContext = ruleset;
    }
}

