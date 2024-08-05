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
            //Rulesets = new(State.Settings.GetAutoRoutingRules().ToLookup(x => x.Id, x => x));
            Rulesets = new(State.Settings.Rulesets);
            Rulesets.CollectionChanged += Rulesets_CollectionChanged;
        }

        private void Rulesets_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //State.Settings.Rulesets = Rulesets.ToList();
        }

        public void NewRuleset(Ruleset ruleset)
        {
            State.Settings.AddRuleset(ruleset);

            Rulesets.Clear();
            State.Settings.Rulesets.ForEach(i => Rulesets.Add(i));
        }

        public void EditRuleset(Ruleset ruleset)
        {
            State.Settings.EditRuleset(ruleset);

            Rulesets.Clear();
            State.Settings.Rulesets.ForEach(i => Rulesets.Add(i));
        }

        public void MoveRulesetUp(Guid Id)
        {
            //var newRulesets = State.Settings.MoveRulesetUp(Id);

            //this.Rulesets = newRulesets;

            // OnPropertyChanged(nameof(Rulesets));


            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(rulesets)));
        }

    }
}
