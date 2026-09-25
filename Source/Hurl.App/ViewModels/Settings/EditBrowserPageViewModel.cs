using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.App.Services.Interfaces;
using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hurl.App.ViewModels
{
    public partial class EditBrowserPageViewModel : ObservableObject
    {
        public Browser Original { get; }
        public bool IsNewBrowser { get; }

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ExePath { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string LaunchArgs { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool Hidden { get; set; } = false;

        [ObservableProperty]
        public partial BrowserIcon? Icon { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasNoIcon))]
        public partial BitmapImage? IconPreview { get; set; }

        public bool HasNoIcon => IconPreview is null;

        [ObservableProperty]
        public partial ObservableCollection<AlternateLaunch> AlternateLaunches { get; set; } = new();

        private readonly ObservableCollection<Browser> browsers;
        private readonly ISettingsService settingsService;
        private readonly IIconLoader iconLoader;
        private bool suppressPreviewRefresh;
        private int previewVersion;

        // Tracks the current refresh so callers can await preview initialization or updates.
        public Task IconPreviewLoadTask { get; private set; } = Task.CompletedTask;

        public EditBrowserPageViewModel(Browser browser, ISettingsService settingsService, IIconLoader iconLoader, bool isNewBrowser = false)
        {
            Original = browser;
            browsers = settingsService.LoadSettings().Browsers;
            this.settingsService = settingsService;
            this.iconLoader = iconLoader;
            IsNewBrowser = isNewBrowser;
            Revert();
        }

        partial void OnExePathChanged(string value) => RefreshIconPreview();

        partial void OnIconChanged(BrowserIcon? value) => RefreshIconPreview();

        private void RefreshIconPreview()
        {
            if (!suppressPreviewRefresh)
                IconPreviewLoadTask = LoadIconPreviewAsync();
        }

        private async Task LoadIconPreviewAsync()
        {
            int version = ++previewVersion;
            try
            {
                var image = await iconLoader.LoadIconAsync(new Browser { ExePath = ExePath, Icon = Icon });
                if (version == previewVersion) IconPreview = image;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could not refresh browser icon preview: {ex.Message}");
                if (version == previewVersion) IconPreview = null;
            }
        }

        public void ApplyIcon(BrowserIcon icon, BitmapImage preview)
        {
            // Reuse the validated chooser image and invalidate any older preview request.
            previewVersion++;
            suppressPreviewRefresh = true;
            try
            {
                Icon = icon.Source == BrowserIconSource.Executable && icon.Index == 0 ? null : icon;
                IconPreview = preview;
                IconPreviewLoadTask = Task.CompletedTask;
            }
            finally
            {
                suppressPreviewRefresh = false;
            }
        }

        private static ObservableCollection<AlternateLaunch> CloneAlternateLaunches(ObservableCollection<AlternateLaunch> launches)
            => new(launches.Select(launch => new AlternateLaunch(launch.ItemName, launch.LaunchArgs) { Id = launch.Id }));

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
            Original.Icon = Icon;
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
            // Restore the draft as a batch so only the final icon configuration is loaded.
            suppressPreviewRefresh = true;
            try
            {
                Name = Original.Name ?? string.Empty;
                ExePath = Original.ExePath ?? string.Empty;
                LaunchArgs = Original.LaunchArgs ?? string.Empty;
                Hidden = Original.Hidden;
                Icon = Original.Icon;
                AlternateLaunches = Original.AlternateLaunches != null
                    ? CloneAlternateLaunches(Original.AlternateLaunches)
                    : new ObservableCollection<AlternateLaunch>();
            }
            finally
            {
                suppressPreviewRefresh = false;
            }
            RefreshIconPreview();
        }
    }
}
