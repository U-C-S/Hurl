using Hurl.Library.Models;
using Hurl.RulesetManager.State;

namespace Hurl.RulesetManager.ViewModels;

class MainWindowViewModel
{
    public MainWindowViewModel()
    {
        Rulesets = SettingsState.GetCurrent.Rulesets;
    }

    public List<Ruleset> Rulesets { get; set; }

    //public void UpdateRuleset(EditRulesetViewModel vm)
    //{
    //    var ruleset = vm.ToRuleSet();
    //    if (ruleset != null)
    //    {
    //        var index = Rulesets.FindIndex(x => x.Id == ruleset.Id);
    //        if (index >= 0)
    //        {
    //            Rulesets[index] = ruleset;
    //        }
    //        else
    //        {
    //            Rulesets.Add(ruleset);
    //        }
    //    }
    //}

    public class IndexedRuleset
    {
        public int Index { get; }

        public Ruleset Ruleset { get; }

        private IndexedRuleset(int i, Ruleset r)
        {
            Index = i;
            Ruleset = r;
        }

        public static List<IndexedRuleset> FromRulesets(List<Ruleset> rulesets)
        {
            return rulesets.Select((x, i) => new IndexedRuleset(i, x)).ToList();
        }
    }
}

