﻿using Hurl.Library;
using Hurl.Library.Models;

namespace Hurl.RulesetManager.ViewModels;

public class EditRulesetViewModel
{
    private readonly Settings _settings;

    public int RulesetIndex { get; }

    public List<Rule> Rules { get; set; }

    public List<string> Browsers { get; set; }

    public List<string>? AltLaunches { get; set; }

    public EditRulesetViewModel(Ruleset set)
    {
        // Make settings a singleton
        _settings = SettingsFile.GetSettings();

        Browsers = _settings.Browsers
                            .Select(x => x.Name)
                            .ToList();
        Rules = set.Rules?
                   .Select(x => new Rule(x))
                   .ToList() ?? new List<Rule>();
        SelectedBrowser = Browsers.IndexOf(set.BrowserName);
        SelectedAltLaunch = set.AltLaunchIndex;
    }

    private int _selectedBrowser;

    public int SelectedBrowser
    {
        get => _selectedBrowser;
        set
        {
            if (value < 0)
            {
                AltLaunches = null;
                SelectedAltLaunch = null;
            }
            else
            {
                _selectedBrowser = value;
                SelectedBrowserChanged(value);
            }
        }
    }

    public int? SelectedAltLaunch { get; set; }

    private void SelectedBrowserChanged(int? value)
    {
        var selected = _settings.Browsers[value ?? 0];

        var altLaunchesWithNone = new List<string> { "< None >" };
        altLaunchesWithNone.AddRange(selected?.AlternateLaunches?
                                              .Select(x => x.ItemName)
                                              .ToList() ?? new List<string>());
        AltLaunches = altLaunchesWithNone;
        SelectedAltLaunch = 0;
    }

    public Ruleset ToRuleSet()
    {
        return new()
        {
            BrowserName = Browsers[SelectedBrowser],
            Rules = Rules.Select(x => x.ToString())
                         .ToList(),
            AltLaunchIndex = SelectedAltLaunch > 0 ? SelectedAltLaunch - 1 : null
        };
    }
}

