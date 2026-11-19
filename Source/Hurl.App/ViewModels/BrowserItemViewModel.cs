using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using WinRT;

namespace Hurl.App.ViewModels;

[GeneratedBindableCustomProperty]
public partial class BrowserItemViewModel(Browser model) : ObservableObject
{
    public Browser Model { get; } = model;

    public string Name => Model.Name;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Opacity))]
    public partial bool IsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool AllowAlternateLaunches { get; set; } = true;

    public double Opacity => IsEnabled ? 1.0 : 0.4;

    public ObservableCollection<AlternateLaunch>? AlternateLaunches => Model.AlternateLaunches;

    [ObservableProperty]
    public partial BitmapImage? Icon { get; set; }
}
