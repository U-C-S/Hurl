using Hurl.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.RulesetManager.State;

class RulesetOps
{
    static Settings _settings
    {
        get
        {
            return SettingsState.GetCurrent;
        }
    }

    public static void AddRuleset(Ruleset ruleset)
    {
        _settings.Rulesets.Add(ruleset);
        SettingsState.Save(_settings);
    }

    public static void UpdateRuleset(int index, Ruleset r)
    {
        _settings.Rulesets[index] = r;
        SettingsState.Save(_settings);
    }

    public static void DeleteRuleset(int index)
    {
        _settings.Rulesets.RemoveAt(index);
        SettingsState.Save(_settings);
    }

    public static void MoveUp(int fromIndex)
    {
        if (fromIndex > 0)
        {
            var toIndex = fromIndex - 1;
            var item = _settings.Rulesets[fromIndex];
            _settings.Rulesets.RemoveAt(fromIndex);
            _settings.Rulesets.Insert(toIndex, item);
            SettingsState.Save(_settings);
        }
    }
}

