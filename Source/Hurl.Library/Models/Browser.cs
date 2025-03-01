using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace Hurl.Library.Models;

public partial class Browser(string Name, string ExePath) : ObservableObject
{
    [ObservableProperty]
    public string name = Name;

    [ObservableProperty]
    public string exePath = ExePath;

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
}

public partial class AlternateLaunch(string ItemName, string LaunchArgs) : ObservableObject
{
    [ObservableProperty]
    public string itemName = ItemName;

    [ObservableProperty]
    public string launchArgs = LaunchArgs;
}