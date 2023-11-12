using Hurl.Library.Models;
using System.Collections.Generic;

namespace Hurl.SettingsApp.ViewModels
{
    public class RulesViewModel
    {
        public List<AutoRoutingRules> rulesets { get; set; }

        public RulesViewModel()
        {
            rulesets = State.Settings.GetAutoRoutingRules();
        }


    }
}
