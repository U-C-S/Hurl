using CommunityToolkit.Mvvm.ComponentModel;

namespace Hurl.Selector.Models;

public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    public bool launchUnderMouse = false;

    [ObservableProperty]
    public bool minimizeOnFocusLoss = true;

    [ObservableProperty]
    public bool noWhiteBorder = false;

    [ObservableProperty]
    public string backgroundType = "mica";

    [ObservableProperty]
    public bool ruleMatching = false;

    [ObservableProperty]
    public int[] windowSize = [420, 210];
}


public enum BackgroundMaterial
{
    Acrylic,
    Mica,
    Solid
}
