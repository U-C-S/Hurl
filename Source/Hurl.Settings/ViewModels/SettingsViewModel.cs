using Hurl.Library.Models;

namespace Hurl.Settings.ViewModels;

public class SettingsViewModel
{
    private AppSettings AppSettings { get; set; }

    public SettingsViewModel()
    {
        AppSettings = State.Settings.AppSettings;
    }

    public bool Option_LaunchUnderMouse
    {
        get => AppSettings.LaunchUnderMouse;
        set
        {
            AppSettings.LaunchUnderMouse = value;
            State.Settings.Set_LaunchUnderMouse(value);
        }
    }

    public bool Option_MinimizeOnFocusLoss
    {
        get => AppSettings.MinimizeOnFocusLoss;
        set
        {
            AppSettings.MinimizeOnFocusLoss = value;
            State.Settings.Set_MinimizeOnFocusLoss(value);
        }
    }

    public bool Option_NoWhiteBorder
    {
        get => AppSettings.NoWhiteBorder;
        set
        {
            State.Settings.Set_NoWhiteBorder(value);
            AppSettings.NoWhiteBorder = value;
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
            string val = value switch
            {
                0 => "mica",
                1 => "acrylic",
                _ => "solid"
            };
            State.Settings.Set_BackgroundType(val);
            AppSettings.BackgroundType = val;
        }
    }
}
