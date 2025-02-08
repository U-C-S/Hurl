using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library;
using Hurl.Selector.Models;
using Hurl.Selector.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hurl.Selector.ViewModels;

partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly string _settingsPath;
    [ObservableProperty]
    private Settings _settings = new();

    public SettingsViewModel()
    {
        _settingsPath = Constants.APP_SETTINGS_MAIN;
        LoadSettingsAsync();
        SetupFileWatcher();
    }

    private async void LoadSettingsAsync()
    {
        try
        {
            Settings = await _settingsService.LoadSettingsAsync();
        }
        catch (Exception)
        {
            // Handle error - maybe set default settings
            Settings = new Settings();
        }
    }

    private CancellationTokenSource _loadingCts = new();
    private FileSystemWatcher _watcher;

    private void SetupFileWatcher()
    {
        _watcher = new FileSystemWatcher
        {
            Path = Path.GetDirectoryName(_settingsPath),
            Filter = Path.GetFileName(_settingsPath),
            NotifyFilter = NotifyFilters.LastWrite
        };

        _watcher.Changed += async (s, e) =>
        {
            // Debounce rapid changes
            Debug.WriteLine(e.FullPath);
            await Task.Delay(500);
            LoadSettingsAsync();
        };

        _watcher.EnableRaisingEvents = true;
    }

    // Add backup/restore
    private void BackupSettings()
    {
        var backupPath = $"{_settingsPath}.bak";
        File.Copy(_settingsPath, backupPath);
    }

    //private async Task RestoreFromBackupAsync()
    //{
    //    var backupPath = $"{_settingsPath}.bak";
    //    if (File.Exists(backupPath))
    //    {
    //        await File.CopyAsync(backupPath, _settingsPath);
    //        await LoadSettingsAsync();
    //    }
    //}
}

