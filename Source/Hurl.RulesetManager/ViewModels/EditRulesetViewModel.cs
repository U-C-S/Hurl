using Hurl.Library.Models;

namespace Hurl.RulesetManager.ViewModels;

public class EditRulesetViewModel
{
    public string? Name { get; set; }

    public List<Rule> Rules { get; set; }

    public List<string> Browsers { get; set; }

    public List<string>? AltLaunches { get; set; }

    public EditRulesetViewModel(Ruleset? set)
    {
        //Index = index;
        Browsers = SettingsState.GetBrowsers()
            .Select(x => x.Name)
            .ToList();
        Name = set?.RulesetName;
        Rules = set?.Rules?
                    .Select(x => new Rule(x))
                    .ToList() ?? new List<Rule>();

        if (set?.BrowserName is string browser)
            SelectedBrowser = Browsers.IndexOf(browser);

        SelectedAltLaunch = set?.AltLaunchIndex != null ? set?.AltLaunchIndex + 1 : null;
    }

    public EditRulesetViewModel()
    {
        Browsers = SettingsState.GetBrowsers()
            .Select(x => x.Name)
            .ToList();
        Rules = new List<Rule>();
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
        var selected = SettingsState.GetBrowsers()[value ?? 0];

        var altLaunchesWithNone = new List<string> { "< None >" };
        altLaunchesWithNone.AddRange(selected?.AlternateLaunches?
                                              .Select(x => x.ItemName)
                                              .ToList() ?? new List<string>());
        AltLaunches = altLaunchesWithNone;
        //SelectedAltLaunch = 0;
    }

    public Ruleset ToRuleSet()
    {
        return new()
        {
            RulesetName = Name,
            BrowserName = Browsers[SelectedBrowser],
            Rules = Rules.Select(x => x.ToString())
                         .ToList(),
            AltLaunchIndex = SelectedAltLaunch > 0 ? SelectedAltLaunch - 1 : null
        };
    }
}

