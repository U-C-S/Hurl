using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hurl.Library.Models;

public partial class Browser : ObservableObject
{
    [ObservableProperty]
    public string name = string.Empty;

    [ObservableProperty]
    public string exePath = string.Empty;

    [ObservableProperty]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool isUwp = false;

    [ObservableProperty]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? launchArgs;

    [ObservableProperty]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ObservableCollection<AlternateLaunch>? alternateLaunches;

    [ObservableProperty]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? customIconPath;

    [ObservableProperty]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool hidden = false;

    [JsonIgnore]
    public ImageSource? Icon
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
                Icon? RawIcon = IconExtractor.FromFile(ExePath.Trim('"'));

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

public partial class AlternateLaunch(string ItemName, string LaunchArgs) : ObservableObject
{
    [ObservableProperty]
    public string itemName = ItemName;

    [ObservableProperty]
    public string launchArgs = LaunchArgs;
}