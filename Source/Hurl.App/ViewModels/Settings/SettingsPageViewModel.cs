using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.App.Services.Interfaces;


namespace Hurl.App.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial AppSettings AppSettings { get; set; }

    private readonly ISettingsService _settingsService;

    public SettingsPageViewModel(ISettingsService settingsService)
    {
        AppSettings = settingsService.LoadSettings().AppSettings;
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

    public int Option_BackgroundType
    {
        get => AppSettings.BackgroundType?.ToLowerInvariant() switch
        {
            "acrylic" => 1,
            _ => 0
        };
        set
        {
            AppSettings.BackgroundType = value switch
            {
                1 => "acrylic",
                _ => "mica"
            };
            _settingsService.UpdateAppSettings(AppSettings);
            OnPropertyChanged();
        }
    }
}
