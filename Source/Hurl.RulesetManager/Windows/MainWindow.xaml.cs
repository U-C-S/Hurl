using Hurl.RulesetManager.Controls;
using Hurl.RulesetManager.Extensions;
using Hurl.RulesetManager.State;

namespace Hurl.RulesetManager.Windows;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        Refresh();
    }

    private int _openIndex = -1;

    public async void Refresh()
    {
        _rulesetControlsList.Children.Clear();

        foreach (var (ruleset, i) in SettingsState.GetCurrent.Rulesets.WithIndex())
        {
            var accordion = new RulesetAccordion(i, ruleset)
            {
                MoveUpRuleset = MoveUpRuleset,
                IsOpen = i == _openIndex
            };
            _rulesetControlsList.Children.Add(accordion);
        }
    }

    public void MoveUpRuleset(int index)
    {
        RulesetOps.MoveUp(index);
        _openIndex = index - 1;

        Refresh();
        //(_rulesetControlsList.Children[index - 1] as RulesetAccordion).MouseUp();
    }
}
