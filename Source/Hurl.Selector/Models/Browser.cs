using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hurl.Selector.Models;

public partial class Browser : ObservableObject
{
    public Browser(string Name, string ExePath)
    {
        this.name = Name;
        this.exePath = ExePath;
    }

    [ObservableProperty]
    public string name = string.Empty;

    [ObservableProperty]
    public string exePath = string.Empty;

    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool isUwp = false;

    [ObservableProperty]
    public string? launchArgs;

    [ObservableProperty]
    public ObservableCollection<AlternateLaunch>? alternateLaunches;

    [ObservableProperty]
    public string? customIconPath;

    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool hidden = false;

    [JsonIgnore]
    public ImageSource? GetIcon
    {
        get
        {
            if (!string.IsNullOrEmpty(customIconPath) && customIconPath.EndsWith(".ico"))
            {
                Icon RawIcon = new(customIconPath, -1, -1);
                return IconUtilites.ToImageSource(RawIcon);
            }
            else if (!string.IsNullOrEmpty(customIconPath))
            {
                return new BitmapImage(new Uri(customIconPath));
            }
            else if (!string.IsNullOrEmpty(exePath))
            {
                Icon? RawIcon = IconExtractor.FromFile(exePath.Trim('"'));

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
            return alternateLaunches == null || alternateLaunches.Count == 0 ? Visibility.Hidden : Visibility.Visible;
        }
    }
}

public partial class AlternateLaunch(string ItemName, string LaunchArgs): ObservableObject
{
    [ObservableProperty]
    public string itemName = ItemName;

    [ObservableProperty]
    public string launchArgs = LaunchArgs;
}
