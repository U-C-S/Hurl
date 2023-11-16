using Hurl.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.Settings.ViewModels
{
    public class RulesetViewModel
    {
        public List<AutoRoutingRules> rulesets { get; set; }

        public RulesetViewModel()
        {
            rulesets = State.Settings.GetAutoRoutingRules();
        }

    }
}
