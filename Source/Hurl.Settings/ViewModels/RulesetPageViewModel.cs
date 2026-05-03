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
    public partial ObservableCollection<Ruleset> Rulesets { get; set; }

    [ObservableProperty]
    public partial AppSettings AppSettings { get; set; }

    public RulesetPageViewModel(IOptions<Library.Models.Settings> settings, ISettingsService settingsService)
    {
        this._settingsService = settingsService;
        Rulesets = new(settings.Value.Rulesets);
        AppSettings = settings.Value.AppSettings;
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
        _settingsService.UpdateRulesets(Rulesets);
    }

    public void EditRuleset(Ruleset ruleset)
    {
        var existingRuleset = Rulesets.First(x => x.Id == ruleset.Id);
        var index = Rulesets.IndexOf(existingRuleset);
        if (index != -1)
        {
            Rulesets[index] = ruleset;
            _settingsService.UpdateRulesets(Rulesets);
        }
    }

    public void MoveRulesetUp(Guid Id)
    {
        var existingRuleset = Rulesets.First(x => x.Id == Id);
        var index = Rulesets.IndexOf(existingRuleset);
        if (index > 0)
        {
            Rulesets.Move(index, index - 1);
            _settingsService.UpdateRulesets(Rulesets);
        }
    }

    public void MoveRulesetDown(Guid Id)
    {
        var existingRuleset = Rulesets.First(x => x.Id == Id);
        var index = Rulesets.IndexOf(existingRuleset);
        if (index != -1 && index < Rulesets.Count - 1)
        {
            Rulesets.Move(index, index + 1);
            _settingsService.UpdateRulesets(Rulesets);
        }
    }

    public void DeleteRuleset(Guid Id)
    {
        var existingRuleset = Rulesets.First(x => x.Id == Id);
        if (existingRuleset != null)
        {
            Rulesets.Remove(existingRuleset);
            _settingsService.UpdateRulesets(Rulesets);
        }
    }

    //private void Refresh()
    //{
    //    Rulesets.Clear();
    //    State.Settings.Rulesets.ForEach(i => Rulesets.Add(i));
    //}
}
