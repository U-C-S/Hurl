using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.Settings.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Hurl.Settings.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private AppSettings appSettings;

    private readonly ISettingsService _settingsService;

    public SettingsViewModel(IOptionsMonitor<Library.Models.Settings> settings, ISettingsService settingsService)
    {
        appSettings = settings.CurrentValue.AppSettings;
        _settingsService = settingsService;
    }


    public bool Option_LaunchUnderMouse
    {
        get => AppSettings.LaunchUnderMouse;
        set
        {
            if (AppSettings.LaunchUnderMouse != value)
            {
                AppSettings.LaunchUnderMouse = value;
                _settingsService.UpdateAppSettings(AppSettings);
                OnPropertyChanged();
            }
        }
    }

    public bool Option_MinimizeOnFocusLoss
    {
        get => AppSettings.MinimizeOnFocusLoss;
        set
        {
            if (AppSettings.MinimizeOnFocusLoss != value)
            {
                AppSettings.MinimizeOnFocusLoss = value;
                _settingsService.UpdateAppSettings(AppSettings);

                OnPropertyChanged();
            }
        }
    }

    public bool Option_NoWhiteBorder
    {
        get => AppSettings.NoWhiteBorder;
        set
        {
            if (AppSettings.NoWhiteBorder != value)
            {
                AppSettings.NoWhiteBorder = value;
                _settingsService.UpdateAppSettings(AppSettings);

                OnPropertyChanged();
            }
        }
    }

    public int Option_BackgroundType
    {
        get => AppSettings.BackgroundType switch
        {
            "mica" => 0,
            "acrylic" => 1,
            _ => 2
        };
        set
        {
            AppSettings.BackgroundType = value switch
            {
                0 => "mica",
                1 => "acrylic",
                _ => "solid"
            };
            _settingsService.UpdateAppSettings(AppSettings);
            OnPropertyChanged();
        }
    }
}
