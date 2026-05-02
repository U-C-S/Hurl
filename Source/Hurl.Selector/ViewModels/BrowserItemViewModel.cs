using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using WinRT;

namespace Hurl.Selector.ViewModels;

[GeneratedBindableCustomProperty]
public partial class BrowserItemViewModel(Browser model) : ObservableObject
{
    public Browser Model { get; } = model;

    public string Name => Model.Name;

    public ObservableCollection<AlternateLaunch>? AlternateLaunches => Model.AlternateLaunches;

    [ObservableProperty]
    public partial BitmapImage? Icon { get; set; }
}
