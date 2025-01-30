using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows;

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

    [ObservableProperty]
    [JsonIgnore]
    public BitmapImage? icon;

    [JsonIgnore]
    public Visibility ShowAdditionalBtn
    {
        get
        {
            return alternateLaunches == null || alternateLaunches.Count == 0 ? Visibility.Hidden : Visibility.Visible;
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
