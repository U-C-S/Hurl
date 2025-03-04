using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
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

    [ObservableProperty]
    [property: JsonIgnore]
    public BitmapImage? icon;
}

public partial class AlternateLaunch(string ItemName, string LaunchArgs) : ObservableObject
{
    [ObservableProperty]
    public string itemName = ItemName;

    [ObservableProperty]
    public string launchArgs = LaunchArgs;
}