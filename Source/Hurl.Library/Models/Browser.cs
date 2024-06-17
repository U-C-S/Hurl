using System;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hurl.Library.Models;

public class Browser(string Name, string ExePath)
{
    public string Name { get; set; } = Name;

    public string ExePath { get; set; } = ExePath;

    public string LaunchArgs { get; set; }

    public AlternateLaunch[] AlternateLaunches { get; set; }

    public string CustomIconPath { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Hidden { get; set; } = false;

    [JsonIgnore]
    public ImageSource GetIcon
    {
        get
        {
            if (!string.IsNullOrEmpty(CustomIconPath) && CustomIconPath.EndsWith(".ico"))
            {
                Icon RawIcon = new(CustomIconPath, -1, -1);
                return IconUtilites.ToImageSource(RawIcon);
            }
            else if (!string.IsNullOrEmpty(CustomIconPath))
            {
                return new BitmapImage(new Uri(CustomIconPath));
            }
            else if (!string.IsNullOrEmpty(ExePath))
            {
                Icon RawIcon = IconExtractor.FromFile(ExePath.Trim('"'));

                return IconUtilites.ToImageSource(RawIcon);
            }
            else
                return null;
        }
    }

    [JsonIgnore]
    public Visibility ShowAdditionalBtn
    {
        get
        {
            return AlternateLaunches == null || AlternateLaunches.Length == 0 ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}

public class AlternateLaunch
{
    public AlternateLaunch() { }

    public AlternateLaunch(string ItemName, string LaunchArgs)
    {
        this.ItemName = ItemName;
        this.LaunchArgs = LaunchArgs;
    }

    public string ItemName { get; set; }

    public string LaunchArgs { get; set; }
}
