using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.Settings.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hurl.Settings.ViewModels;

public partial class RulesetPageViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    private ObservableCollection<Ruleset> rulesets;

    [ObservableProperty]
    private AppSettings appSettings;

    public RulesetPageViewModel(IOptions<Library.Models.Settings> settings, ISettingsService settingsService)
    {
        this._settingsService = settingsService;
        rulesets = new(settings.Value.Rulesets);
        appSettings = settings.Value.AppSettings;
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
        rulesets.Add(ruleset);
        _settingsService.UpdateRulesets(rulesets);
    }

    public void EditRuleset(Ruleset ruleset)
    {
        var existingRuleset = rulesets.First(x => x.Id == ruleset.Id);
        var index = rulesets.IndexOf(existingRuleset);
        if (index != -1)
        {
            rulesets[index] = ruleset;
            _settingsService.UpdateRulesets(rulesets);
        }
    }

    public void MoveRulesetUp(Guid Id)
    {
        var existingRuleset = rulesets.First(x => x.Id == Id);
        var index = rulesets.IndexOf(existingRuleset);
        if (index > 0)
        {
            rulesets.Move(index, index - 1);
            _settingsService.UpdateRulesets(rulesets);
        }
    }

    public void MoveRulesetDown(Guid Id)
    {
        var existingRuleset = rulesets.First(x => x.Id == Id);
        var index = rulesets.IndexOf(existingRuleset);
        if (index != -1 && index < rulesets.Count - 1)
        {
            rulesets.Move(index, index + 1);
            _settingsService.UpdateRulesets(rulesets);
        }
    }

    public void DeleteRuleset(Guid Id)
    {
        var existingRuleset = rulesets.First(x => x.Id == Id);
        if (existingRuleset != null)
        {
            rulesets.Remove(existingRuleset);
            _settingsService.UpdateRulesets(rulesets);
        }
    }

    //private void Refresh()
    //{
    //    Rulesets.Clear();
    //    State.Settings.Rulesets.ForEach(i => Rulesets.Add(i));
    //}
}
