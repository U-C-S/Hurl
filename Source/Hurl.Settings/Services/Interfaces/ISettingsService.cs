using Hurl.Library.Models;
using System.Collections.ObjectModel;

namespace Hurl.Settings.Services.Interfaces;

public interface ISettingsService
{
    //Task<Settings> LoadSettingsAsync();

    void SaveSettings(Library.Models.Settings settings);

    void UpdateAppSettings(AppSettings appSettings);

    void UpdateQuickView(QuickViewSettings quickView);

    void UpdateBrowsers(ObservableCollection<Browser> browsers);

    void UpdateRulesets(ObservableCollection<Ruleset> rulesets);
}
