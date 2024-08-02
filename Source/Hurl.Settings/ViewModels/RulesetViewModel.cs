using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hurl.Settings.ViewModels
{
    public partial class RulesetViewModel : ObservableObject
    {
        public List<Ruleset> Rulesets { get; set; }

        public RulesetViewModel()
        {
            //Rulesets = new(State.Settings.GetAutoRoutingRules().ToLookup(x => x.Id, x => x));
            Rulesets = State.Settings.GetAutoRoutingRules();
        }

        public void MoveRulesetUp(int Id)
        {
            var newRulesets = State.Settings.MoveRulesetUp(Id);

            this.Rulesets = newRulesets;

            // OnPropertyChanged(nameof(Rulesets));


            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(rulesets)));
        }

    }
}
