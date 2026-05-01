using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models;

public partial class Browser : ObservableObject
{
    public Browser(string Name, string ExePath)
    {
        this.Name = Name;
        this.ExePath = ExePath;
    }

    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ExePath { get; set; } = string.Empty;

    [ObservableProperty]
    [field: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public partial bool IsUwp { get; set; } = false;

    [ObservableProperty]
    public partial string? LaunchArgs { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<AlternateLaunch>? AlternateLaunches { get; set; }

    [ObservableProperty]
    public partial string? CustomIconPath { get; set; }

    [ObservableProperty]
    [field: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public partial bool Hidden { get; set; } = false;
}

public partial class AlternateLaunch(string ItemName, string LaunchArgs) : ObservableObject
{
    [ObservableProperty]
    public partial string ItemName { get; set; } = ItemName;

    [ObservableProperty]
    public partial string LaunchArgs { get; set; } = LaunchArgs;
}
