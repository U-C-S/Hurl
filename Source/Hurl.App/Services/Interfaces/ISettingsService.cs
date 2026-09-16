using Hurl.Library.Models;
using System;
using System.Collections.ObjectModel;

namespace Hurl.App.Services.Interfaces;

public interface ISettingsService
{
    Settings LoadSettings();
    event EventHandler? SettingsChanged;
    void UpdateAppSettings(AppSettings appSettings);
    void UpdateQuickView(QuickViewSettings quickView);
    void UpdateBrowsers(ObservableCollection<Browser> browsers);
    void UpdateRulesets(ObservableCollection<Ruleset> rulesets);
}
