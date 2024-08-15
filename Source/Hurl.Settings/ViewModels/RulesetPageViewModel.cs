using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using System;
using System.Collections.ObjectModel;

namespace Hurl.Settings.ViewModels;

public partial class RulesetPageViewModel : ObservableObject
{
    public ObservableCollection<Ruleset> Rulesets { get; set; }

    public RulesetPageViewModel()
    {
        Rulesets = new(State.Settings.Rulesets);
    }

    public bool Option_RuleMatching
    {
        get => State.Settings.AppSettings.RuleMatching;
        set
        {
            State.Settings.Set_RuleMatching(value);
        }
    }

    public void NewRuleset(Ruleset ruleset)
    {
        State.Settings.AddRuleset(ruleset);
        Refresh();
    }

    public void EditRuleset(Ruleset ruleset)
    {
        State.Settings.EditRuleset(ruleset);
        Refresh();
    }

    public void MoveRulesetUp(Guid Id)
    {
        State.Settings.MoveRulesetUp(Id);
        Refresh();
    }

    public void MoveRulesetDown(Guid Id)
    {
        State.Settings.MoveRulesetDown(Id);
        Refresh();
    }

    public void DeleteRuleset(Guid Id)
    {
        State.Settings.DeleteRuleset(Id);
        Refresh();
    }

    private void Refresh()
    {
        Rulesets.Clear();
        State.Settings.Rulesets.ForEach(i => Rulesets.Add(i));
    }
}
