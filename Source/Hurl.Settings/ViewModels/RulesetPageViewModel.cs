using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.Settings.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hurl.Settings.ViewModels;

public partial class RulesetPageViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly List<Browser> browsers;

    [ObservableProperty]
    public partial ObservableCollection<Ruleset> Rulesets { get; set; }

    public ObservableCollection<RulesetItemViewModel> RulesetItems { get; } = [];

    [ObservableProperty]
    public partial AppSettings AppSettings { get; set; }

    public RulesetPageViewModel(IOptions<Library.Models.Settings> settings, ISettingsService settingsService)
    {
        _settingsService = settingsService;
        browsers = settings.Value.Browsers.ToList();
        Rulesets = new(settings.Value.Rulesets);
        AppSettings = settings.Value.AppSettings;
        RefreshRulesetItems();
    }

    public bool Option_RuleMatching
    {
        get => AppSettings.RuleMatching;
        set
        {
            if (AppSettings.RuleMatching != value)
            {
                AppSettings.RuleMatching = value;
                _settingsService.UpdateAppSettings(AppSettings);
                OnPropertyChanged();
            }
        }
    }

    public void NewRuleset(Ruleset ruleset)
    {
        Rulesets.Add(ruleset);
        SaveRulesets();
    }

    public void EditRuleset(Ruleset ruleset)
    {
        var existingRuleset = Rulesets.First(x => x.Id == ruleset.Id);
        var index = Rulesets.IndexOf(existingRuleset);
        if (index != -1)
        {
            Rulesets[index] = ruleset;
            SaveRulesets();
        }
    }

    public void MoveRulesetUp(Guid Id)
    {
        var existingRuleset = Rulesets.First(x => x.Id == Id);
        var index = Rulesets.IndexOf(existingRuleset);
        if (index > 0)
        {
            Rulesets.Move(index, index - 1);
            SaveRulesets();
        }
    }

    public void MoveRulesetDown(Guid Id)
    {
        var existingRuleset = Rulesets.First(x => x.Id == Id);
        var index = Rulesets.IndexOf(existingRuleset);
        if (index != -1 && index < Rulesets.Count - 1)
        {
            Rulesets.Move(index, index + 1);
            SaveRulesets();
        }
    }

    public void DeleteRuleset(Guid Id)
    {
        var existingRuleset = Rulesets.First(x => x.Id == Id);
        if (existingRuleset != null)
        {
            Rulesets.Remove(existingRuleset);
            SaveRulesets();
        }
    }

    public Ruleset GetRuleset(Guid id)
    {
        return Rulesets.First(x => x.Id == id);
    }

    public string GetBrowserDisplayName(Guid browserId)
    {
        return browsers.FirstOrDefault(browser => browser.Id == browserId)?.Name
            ?? "Missing browser";
    }

    private void SaveRulesets()
    {
        _settingsService.UpdateRulesets(Rulesets);
        RefreshRulesetItems();
    }

    private void RefreshRulesetItems()
    {
        RulesetItems.Clear();

        foreach (Ruleset ruleset in Rulesets)
        {
            RulesetItems.Add(new RulesetItemViewModel(
                ruleset,
                GetBrowserDisplayName(ruleset.BrowserId)));
        }
    }
}

public sealed record RulesetItemViewModel(Ruleset Model, string BrowserDisplayName);
