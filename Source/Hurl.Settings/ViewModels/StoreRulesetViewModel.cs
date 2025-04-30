using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hurl.Settings.ViewModels;

public class StoreRulesetViewModel : ObservableObject
{
    public readonly Guid Id;

    public readonly List<string> Browsers;

    public List<string> AltLaunches { get; set; } = ["< None >"];

    public string? Name { get; set; }

    public List<Rule> Rules { get; set; }

    public StoreRulesetViewModel(IOptionsMonitor<Library.Models.Settings> settings)
    {
        Id = Guid.NewGuid();
        Browsers = settings.CurrentValue.Browsers
            .Select(x => x.Name)
            .ToList();
        Rules = [];
    }

    public StoreRulesetViewModel(IOptionsMonitor<Library.Models.Settings> settings, Guid id)
    {
        Id = id;
        var currentRuleset = settings.CurrentValue.Rulesets.First(x => Guid.Equals(x.Id, id));
        Browsers = settings.CurrentValue.Browsers
            .Select(x => x.Name)
            .ToList();
        Name = currentRuleset?.RulesetName;
        Rules = currentRuleset?.Rules?
                    .Select(x => new Rule(x))
                    .ToList() ?? [];

        if (currentRuleset?.BrowserName is string browser)
            SelectedBrowser = Browsers.IndexOf(browser);

        if (currentRuleset?.AltLaunchIndex is int altLaunchIndex)
        {
            List<string> altLaunchList = ["< None >"];
            var x = settings.CurrentValue.Browsers[SelectedBrowser]
                ?.AlternateLaunches
                ?.Select(x => x.ItemName)
                .ToList();
            altLaunchList.AddRange(x);
            AltLaunches = altLaunchList;
            SelectedAltLaunch = altLaunchIndex + 1;
        }
    }

    private int _selectedBrowser = -1;

    public int SelectedBrowser
    {
        get => _selectedBrowser;
        set
        {
            if (value < 0)
            {
                SelectedAltLaunch = 0;
                AltLaunches = ["< None >"];
            }
            else
            {
                _selectedBrowser = value;
                SelectedBrowserChanged(value);
            }
        }
    }

    public int SelectedAltLaunch { get; set; } = 0;

    private void SelectedBrowserChanged(int value)
    {
        //var selected = State.Settings.GetBrowsers()[value];
        //if (selected.AlternateLaunches?.Count > 0)
        //{
        //    List<string> altLaunchList = ["< None >"];
        //    var x = selected
        //        .AlternateLaunches
        //        .Select(x => x.ItemName)
        //        .ToList();
        //    altLaunchList.AddRange(x);
        //    AltLaunches = altLaunchList;
        //}
        //else
        //{
        //    AltLaunches = ["< None >"];
        //}
    }

    public Ruleset ToRuleSet()
    {
        return new()
        {
            Id = Id,
            RulesetName = Name ?? "",
            BrowserName = Browsers[SelectedBrowser],
            Rules = Rules.Select(x => x.ToString()).ToList(),
            AltLaunchIndex = SelectedAltLaunch > 0 ? SelectedAltLaunch - 1 : null
        };
    }

}
