using Hurl.Library.Models;

namespace Hurl.SettingsApp.ViewModels
{
    public class SettingsViewModel
    {
        private AppSettings appSettings { get; set; }
        public SettingsViewModel()
        {
            appSettings = State.Settings.GetAppSettings();
            //Option_LaunchUnderMouse = x.LaunchUnderMouse;
            //Option_NoWhiteBorder = x.NoWhiteBorder;
        }

        public bool Option_LaunchUnderMouse
        {
            get => appSettings.LaunchUnderMouse;
            set
            {
                appSettings.LaunchUnderMouse = value;
                State.Settings.Set_LaunchUnderMouse(value);
            }
        }

        public bool Option_NoWhiteBorder
        {
            get => appSettings.NoWhiteBorder;
            set
            {
                State.Settings.Set_NoWhiteBorder(value);
                appSettings.NoWhiteBorder = value;
            }
        }

        public int Option_BackgroundType
        {
            get => appSettings.BackgroundType switch
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
                appSettings.BackgroundType = val;
            }
        }
    }
}
