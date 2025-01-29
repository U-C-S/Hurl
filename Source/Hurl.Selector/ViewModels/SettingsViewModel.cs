using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Selector.Models;
using Hurl.Selector.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        LoadSettingsAsync();
    }

    private async void LoadSettingsAsync()
    {
        try
        {
            Settings = await _settingsService.LoadSettingsAsync();
        }
        catch (Exception ex)
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

