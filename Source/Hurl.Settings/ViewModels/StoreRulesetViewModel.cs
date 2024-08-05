using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
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

    public StoreRulesetViewModel()
    {
        Id = Guid.NewGuid();
        Browsers = State.Settings.GetBrowsers()
            .Select(x => x.Name)
            .ToList();
        Rules = [];
    }

    public StoreRulesetViewModel(Ruleset set)
    {
        Id = set.Id;
        Browsers = State.Settings.GetBrowsers()
            .Select(x => x.Name)
            .ToList();
        Name = set?.RulesetName;
        Rules = set?.Rules?
                    .Select(x => new Rule(x))
                    .ToList() ?? [];

        if (set?.BrowserName is string browser)
            SelectedBrowser = Browsers.IndexOf(browser);

        if (set?.AltLaunchIndex is int altLaunchIndex)
        {
            List<string> altLaunchList = ["< None >"];
            var x = State.Settings.GetBrowsers()[SelectedBrowser]
                .AlternateLaunches
                .Select(x => x.ItemName)
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
        var selected = State.Settings.GetBrowsers()[value];
        if (selected.AlternateLaunches?.Count > 0)
        {
            List<string> altLaunchList = ["< None >"];
            var x = selected
                .AlternateLaunches
                .Select(x => x.ItemName)
                .ToList();
            altLaunchList.AddRange(x);
            AltLaunches = altLaunchList;
        }
        else
        {
            AltLaunches = ["< None >"];
        }
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
