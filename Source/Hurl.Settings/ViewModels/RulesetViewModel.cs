using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using System.Linq;

namespace Hurl.Settings.ViewModels
{
    public partial class RulesetViewModel : ObservableObject
    {
        public ObservableGroupedCollection<int, Ruleset> Rulesets { get; set; } = new();

        public RulesetViewModel()
        {
            Rulesets = new(State.Settings.GetAutoRoutingRules().ToLookup(x => x.Id, x => x));
        }

        public void MoveRulesetUp(int Id)
        {
            var newRulesets = State.Settings.MoveRulesetUp(Id);

            this.Rulesets = new(newRulesets.ToLookup(x => x.Id, x => x));

            // OnPropertyChanged(nameof(Rulesets));


            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(rulesets)));
        }

    }
}
