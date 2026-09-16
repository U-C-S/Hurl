using Hurl.App.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hurl.App.ViewModels;

public class StoreRulesetViewModel : ObservableObject
{
    private static readonly AlternateLaunchOption NoAlternateLaunch = new(null, "< None >");

    public Guid Id { get; } = Guid.NewGuid();
    public List<Browser> Browsers { get; }
    public ObservableCollection<AlternateLaunchOption> AltLaunches { get; } = [NoAlternateLaunch];
    public string? Name { get; set; }
    public List<Rule> Rules { get; set; } = [];

    public StoreRulesetViewModel(ISettingsService settingsService)
    {
        Browsers = settingsService.LoadSettings().Browsers.ToList();
    }

    public StoreRulesetViewModel(ISettingsService settingsService, Guid id)
        : this(settingsService)
    {
        var ruleset = settingsService.LoadSettings().Rulesets.First(rule => rule.Id == id);
        Id = id;
        Name = ruleset.RulesetName;
        Rules = ruleset.Rules?.Select(rule => new Rule(rule)).ToList() ?? [];
        SelectedBrowser = Browsers.FirstOrDefault(browser => browser.Id == ruleset.BrowserId);
        SelectedAltLaunch = AltLaunches.FirstOrDefault(launch => launch.Id == ruleset.AlternateLaunchId)
            ?? NoAlternateLaunch;
    }

    private Browser? selectedBrowser;
    public Browser? SelectedBrowser
    {
        get => selectedBrowser;
        set
        {
            if (selectedBrowser == value)
            {
                return;
            }

            selectedBrowser = value;
            AltLaunches.Clear();
            AltLaunches.Add(NoAlternateLaunch);
            foreach (var launch in value?.AlternateLaunches ?? [])
            {
                AltLaunches.Add(new AlternateLaunchOption(launch.Id, launch.ItemName));
            }
            SelectedAltLaunch = NoAlternateLaunch;
            OnPropertyChanged();
        }
    }

    private AlternateLaunchOption? selectedAltLaunch = NoAlternateLaunch;
    public AlternateLaunchOption? SelectedAltLaunch
    {
        get => selectedAltLaunch;
        set => SetProperty(ref selectedAltLaunch, value);
    }

    public Ruleset ToRuleSet() => new()
    {
        Id = Id,
        RulesetName = Name ?? "",
        BrowserId = SelectedBrowser?.Id ?? Guid.Empty,
        AlternateLaunchId = SelectedAltLaunch?.Id,
        Rules = Rules.Select(rule => rule.ToString()).ToList()
    };
}

public sealed record AlternateLaunchOption(Guid? Id, string DisplayName);
