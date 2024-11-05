using Hurl.Library;
using Hurl.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hurl.Settings.State;

public partial class Settings
{
    private static Settings _instance = new();

    private Library.Models.Settings _data = Library.Models.Settings.GetSettings();

    private static Library.Models.Settings Value
    {
        get
        {
            return _instance._data;
        }
        set
        {
            _instance._data = value;
        }
    }

    #region AppSettingsMethods

    public static AppSettings AppSettings => Value.AppSettings;

    public static void Set_LaunchUnderMouse(bool value)
    {
        Value.AppSettings.LaunchUnderMouse = value;
        SaveToJson();
    }

    public static void Set_MinimizeOnFocusLoss(bool value)
    {
        Value.AppSettings.MinimizeOnFocusLoss = value;
        SaveToJson();
    }

    public static void Set_NoWhiteBorder(bool value)
    {
        Value.AppSettings.NoWhiteBorder = value;
        SaveToJson();
    }

    public static void Set_BackgroundType(string value)
    {
        Value.AppSettings.BackgroundType = value;
        SaveToJson();
    }

    public static void Set_RuleMatching(bool value)
    {
        Value.AppSettings.RuleMatching = value;
        SaveToJson();
    }

    #endregion

    #region BrowserMethods

    public static List<Browser> Browsers
    {
        get
        {
            return Value.Browsers;
        }
        set
        {
            Value.Browsers = value;
            SaveToJson();
        }
    }

    public static List<Browser> GetBrowsers()
    {
        return Library.GetBrowsers.FromSettingsFile(Value, false);
    }

    public static List<string> GetBrowserNames()
    {
        return Value.Browsers.AsQueryable().Select(x => x.Name).ToList();
    }

    public static List<string> GetAltLaunchesNamesForBrowser(string name)
    {
        var selectedBrowser = from browser in Value.Browsers
                              where browser.Name == name
                              select browser;
        var altLaunchesStrings = from alt in selectedBrowser.First().AlternateLaunches
                                 select alt.ItemName;

        return altLaunchesStrings.ToList();
    }

    public static void RefreshBrowsers()
    {
        var refreshedBrowsers = Library.GetBrowsers.FromRegistry();
        var newList = Browsers;

        // Go over the new browser list and add any of those browsers that are not already present
        // in the existing browser list
        foreach (var newBrowser in refreshedBrowsers)
        {
            var isExists = Browsers.Any(b => b.ExePath == newBrowser.ExePath);
            if (!isExists)
            {
                newList.Add(newBrowser);
            }
        }

        Browsers = newList;
    }

    #endregion

    #region RulesetMethods

    public static List<Ruleset> Rulesets
    {
        get
        {
            return Value.Rulesets;
        }
        set
        {
            Value.Rulesets = value;
            SaveToJson();
        }
    }

    public static List<Ruleset> GetAutoRoutingRules()
    {
        return Value.Rulesets;
    }

    public static void AddRuleset(Ruleset set)
    {
        Value.Rulesets.Add(set);
        SaveToJson();
    }

    public static void EditRuleset(Ruleset set)
    {
        var ruleset = Value.Rulesets.Where(x => x.Id == set.Id).First();
        var index = Value.Rulesets.IndexOf(ruleset);

        Value.Rulesets[index] = set;
        SaveToJson();
    }

    internal static void MoveRulesetUp(Guid id)
    {
        var ruleset = Value.Rulesets.Where(x => x.Id == id).First();
        var index = Value.Rulesets.IndexOf(ruleset);

        if (index > 0)
        {
            Value.Rulesets.Remove(ruleset);
            Value.Rulesets.Insert(index - 1, ruleset);
        }

        SaveToJson();
    }

    internal static void MoveRulesetDown(Guid id)
    {
        var ruleset = Value.Rulesets.Where(x => x.Id == id).First();
        var index = Value.Rulesets.IndexOf(ruleset);

        if (index < Value.Rulesets.Count - 1)
        {
            Value.Rulesets.Remove(ruleset);
            Value.Rulesets.Insert(index + 1, ruleset);
        }

        SaveToJson();
    }

    internal static void DeleteRuleset(Guid id)
    {
        var ruleset = Value.Rulesets.Where(x => x.Id == id).First();
        Value.Rulesets.Remove(ruleset);

        SaveToJson();
    }

    #endregion

    private static void SaveToJson()
    {
        JsonOperations.FromModelToJson(Value, Constants.APP_SETTINGS_MAIN);
    }
}
