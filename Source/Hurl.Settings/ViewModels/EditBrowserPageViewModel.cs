using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.Settings.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.Settings.ViewModels
{
    public partial class EditBrowserPageViewModel : ObservableObject
    {
        public Browser Original { get; }
        public bool IsNewBrowser { get; }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string exePath = string.Empty;

        [ObservableProperty]
        private string launchArgs = string.Empty;

        [ObservableProperty]
        private bool hidden = false;

        [ObservableProperty]
        private string? customIconPath;

        [ObservableProperty]
        private ObservableCollection<AlternateLaunch> alternateLaunches = new();

        private readonly ObservableCollection<Browser> browsers;
        private readonly ISettingsService settingsService;

        public EditBrowserPageViewModel(Browser browser, IOptionsMonitor<Library.Models.Settings> options, ISettingsService settingsService, bool isNewBrowser = false)
        {
            Original = browser;
            browsers = options.CurrentValue.Browsers;
            this.settingsService = settingsService;
            IsNewBrowser = isNewBrowser;

            // Initialize editable fields from the original browser
            Name = browser.Name ?? string.Empty;
            ExePath = browser.ExePath ?? string.Empty;
            LaunchArgs = browser.LaunchArgs ?? string.Empty;
            Hidden = browser.Hidden;
            CustomIconPath = browser.CustomIconPath;
            AlternateLaunches = browser.AlternateLaunches != null
                ? new ObservableCollection<AlternateLaunch>(browser.AlternateLaunches)
                : new ObservableCollection<AlternateLaunch>();
        }

        public void AddAlternate(string itemName, string args)
        {
            if (string.IsNullOrWhiteSpace(itemName) && string.IsNullOrWhiteSpace(args))
                return;

            AlternateLaunches.Add(new AlternateLaunch(itemName ?? string.Empty, args ?? string.Empty));
        }

        public void RemoveAlternate(AlternateLaunch alt)
        {
            if (alt == null) return;
            AlternateLaunches.Remove(alt);
        }

        public void Save()
        {
            // Copy edited values back onto the original browser object (in-place)
            Original.Name = Name;
            Original.ExePath = ExePath;
            Original.LaunchArgs = string.IsNullOrWhiteSpace(LaunchArgs) ? null : LaunchArgs;
            Original.Hidden = Hidden;
            Original.CustomIconPath = string.IsNullOrWhiteSpace(CustomIconPath) ? null : CustomIconPath;
            Original.AlternateLaunches = AlternateLaunches.Count > 0
                ? new ObservableCollection<AlternateLaunch>(AlternateLaunches)
                : null;

            if (IsNewBrowser && !browsers.Contains(Original))
            {
                browsers.Add(Original);
            }

            // Persist the browsers collection via the settings service
            settingsService.UpdateBrowsers(browsers);
        }

        public void Revert()
        {
            // Re-load values from the original browser (discard unsaved edits)
            Name = Original.Name ?? string.Empty;
            ExePath = Original.ExePath ?? string.Empty;
            LaunchArgs = Original.LaunchArgs ?? string.Empty;
            Hidden = Original.Hidden;
            CustomIconPath = Original.CustomIconPath;
            AlternateLaunches = Original.AlternateLaunches != null
                ? new ObservableCollection<AlternateLaunch>(Original.AlternateLaunches)
                : new ObservableCollection<AlternateLaunch>();
        }
    }
}
