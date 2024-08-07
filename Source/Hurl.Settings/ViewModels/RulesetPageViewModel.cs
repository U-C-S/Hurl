using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hurl.Settings.ViewModels
{
    public partial class RulesetPageViewModel : ObservableObject
    {
        public ObservableCollection<Ruleset> Rulesets { get; set; }

        public RulesetPageViewModel()
        {
            Rulesets = new(State.Settings.Rulesets);
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
            var newRulesets = State.Settings.MoveRulesetUp(Id);
            Refresh();
        }

        public void MoveRulesetDown(Guid Id)
        {
            var newRulesets = State.Settings.MoveRulesetDown(Id);
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
}
