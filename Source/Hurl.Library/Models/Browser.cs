using System.Drawing;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hurl.Library.Models;

public class Browser
{
    public Browser(string Name, string ExePath)
    {
        this.Name = Name;
        this.ExePath = ExePath;
    }

    public string Name { get; set; }

    public string ExePath { get; set; }

    public string? LaunchArgs { get; set; }

    public List<AlternateLaunch>? AlternateLaunches { get; set; }

    public string? CustomIconPath { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Hidden { get; set; } = false;

    [JsonIgnore]
    public ImageSource? GetIcon
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
            return AlternateLaunches == null || AlternateLaunches.Count == 0 ? Visibility.Hidden : Visibility.Visible;
        }
    }
}

public class AlternateLaunch(string ItemName, string LaunchArgs)
{
    public string ItemName { get; set; } = ItemName;

    public string LaunchArgs { get; set; } = LaunchArgs;
}
