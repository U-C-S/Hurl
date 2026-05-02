using CommunityToolkit.Mvvm.ComponentModel;

namespace Hurl.Library.Models;

public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    public partial bool LaunchUnderMouse { get; set; } = false;

    [ObservableProperty]
    public partial bool MinimizeOnFocusLoss { get; set; } = true;

    [ObservableProperty]
    public partial bool NoWhiteBorder { get; set; } = false;

    [ObservableProperty]
    public partial string BackgroundType { get; set; } = "mica";

    [ObservableProperty]
    public partial bool RuleMatching { get; set; } = false;

    [ObservableProperty]
    public partial int[] WindowSize { get; set; } = [500, 260];
}


public enum BackgroundMaterial
{
    Acrylic,
    Mica,
    Solid
}
