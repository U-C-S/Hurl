using Hurl.Library.Models;
using Hurl.RulesetManager.State;
using Hurl.RulesetManager.ViewModels;
using Hurl.RulesetManager.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.RulesetManager.Controls;

public partial class RulesetAccordion : UserControl
{
    private readonly int _index;

    public Action<int>? MoveUpRuleset;

    public bool IsOpen { get; internal set; }

    public RulesetAccordion(int index, Ruleset ruleset)
    {
        _index = index;
        InitializeComponent();

        DataContext = ruleset;
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
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

        RulesetOps.UpdateRuleset(_index, ruleset);
    }

    private void MoveUpButton_Click(object sender, RoutedEventArgs e)
    {
        MoveUpRuleset?.Invoke(_index);
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {

    }
}

