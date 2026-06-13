using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models;

public partial class QuickViewSettings : ObservableObject
{
    [ObservableProperty]
    public partial bool Enabled { get; set; } = true;

    [ObservableProperty]
    [property: JsonConverter(typeof(JsonStringEnumConverter<QuickViewLaunchMode>))]
    public partial QuickViewLaunchMode LaunchMode { get; set; } = QuickViewLaunchMode.WebView;

    [ObservableProperty]
    [property: JsonConverter(typeof(JsonStringEnumConverter<QuickViewModifierKeys>))]
    public partial QuickViewModifierKeys ModifierKeys { get; set; } = QuickViewModifierKeys.Alt;

    [ObservableProperty]
    public partial Guid? BrowserId { get; set; }

    [ObservableProperty]
    public partial Guid? AlternateLaunchId { get; set; }

    [ObservableProperty]
    public partial string AdditionalBrowserArguments { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool BrowserExtensionsEnabled { get; set; } = false;

    [ObservableProperty]
    [property: JsonConverter(typeof(JsonStringEnumConverter<QuickViewTrackingPreventionLevel>))]
    public partial QuickViewTrackingPreventionLevel TrackingPrevention { get; set; } = QuickViewTrackingPreventionLevel.Balanced;
}

public enum QuickViewLaunchMode
{
    WebView,
    Browser
}

public enum QuickViewModifierKeys
{
    Alt,
    CtrlAlt,
    Ctrl
}

public enum QuickViewTrackingPreventionLevel
{
    None,
    Basic,
    Balanced,
    Strict
}
