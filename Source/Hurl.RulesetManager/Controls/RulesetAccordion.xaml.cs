using Hurl.Library.Models;
using Hurl.RulesetManager.ViewModels;
using Hurl.RulesetManager.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.RulesetManager.Controls;

public partial class RulesetAccordion : UserControl
{
    private readonly int _index;

    public Func<Task>? Refresh;

    public RulesetAccordion(int index, Ruleset ruleset)
    {
        _index = index;
        InitializeComponent();

        DataContext = ruleset;
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
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

        SettingsState.Rulesets[_index] = ruleset;
        SettingsState.Update();
        Refresh?.Invoke();
    }

    private void UpButton_Click(object sender, RoutedEventArgs e)
    {
        var ruleset = ((Ruleset)DataContext);
        var index = _index;
        if (index < 1)
        {
            return;
        }
        else
        {
            var temp = SettingsState.Rulesets[index];
            SettingsState.Rulesets[index] = SettingsState.Rulesets[index - 1];
            SettingsState.Rulesets[index - 1] = temp;
            SettingsState.Update();
            Refresh?.Invoke();
        }
    }

    private void DownButton_Click(object sender, RoutedEventArgs e)
    {
        var ruleset = ((Ruleset)DataContext);
        var index = _index;
        if (index >= SettingsState.Rulesets.Count - 1)
        {
            return;
        }
        else
        {
            var temp = SettingsState.Rulesets[index];
            SettingsState.Rulesets[index] = SettingsState.Rulesets[index + 1];
            SettingsState.Rulesets[index + 1] = temp;
            SettingsState.Update();
            Refresh?.Invoke();
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Are you sure you want to delete this ruleset?", "Delete Ruleset", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            SettingsState.Rulesets.RemoveAt(_index);
            SettingsState.Update();
            Refresh?.Invoke();
        }
    }
}

