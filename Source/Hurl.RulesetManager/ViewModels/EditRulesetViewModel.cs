using Hurl.Library;
using Hurl.Library.Models;

namespace Hurl.RulesetManager.ViewModels;

public class EditRulesetViewModel
{
    public List<Rule> Rules { get; set; }

    public List<string> Browsers { get; set; }

    public List<string>? AltLaunches { get; set; }

    public EditRulesetViewModel(Ruleset? set)
    {
        Browsers = SettingsFile.GetSettings()
                               .Browsers
                               .Select(x => x.Name)
                               .ToList();
        Rules = set?.Rules?
                    .Select(x => new Rule(x))
                    .ToList() ?? new List<Rule>();
        SelectedBrowser = Browsers.IndexOf(set?.BrowserName);
        SelectedAltLaunch = set?.AltLaunchIndex;
    }

    private int? _selectedBrowser;

    public int? SelectedBrowser
    {
        get => _selectedBrowser;
        set
        {
            if (value == null || value < 0)
            {
                AltLaunches = null;
                SelectedAltLaunch = null;
                _selectedBrowser = null;
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
        var selected = SettingsFile.GetSettings()
                                   .Browsers[value ?? 0];

        var altLaunchesWithNone = new List<string> { "< None >" };
        altLaunchesWithNone.AddRange(selected?.AlternateLaunches?
                                              .Select(x => x.ItemName)
                                              .ToList() ?? new List<string>());
        AltLaunches = altLaunchesWithNone;
        SelectedAltLaunch = 0;
    }
}

