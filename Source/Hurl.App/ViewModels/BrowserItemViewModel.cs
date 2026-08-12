using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Globalization;
using WinRT;

namespace Hurl.App.ViewModels;

[GeneratedBindableCustomProperty]
public partial class BrowserItemViewModel(Browser model) : ObservableObject
{
    public Browser Model { get; } = model;

    public string Name => Model.Name;

    public ObservableCollection<AlternateLaunch>? AlternateLaunches => Model.AlternateLaunches;

    [ObservableProperty]
    public partial int? ShortcutIndex { get; set; }

    public string ShortcutHintText => ShortcutIndex is int shortcutIndex && shortcutIndex >= 0 && shortcutIndex < 9
        ? (shortcutIndex + 1).ToString(CultureInfo.InvariantCulture)
        : string.Empty;

    public Visibility ShortcutHintVisibility => string.IsNullOrEmpty(ShortcutHintText)
        ? Visibility.Collapsed
        : Visibility.Visible;

    [ObservableProperty]
    public partial BitmapImage? Icon { get; set; }

    partial void OnShortcutIndexChanged(int? value)
    {
        OnPropertyChanged(nameof(ShortcutHintText));
        OnPropertyChanged(nameof(ShortcutHintVisibility));
    }
}
