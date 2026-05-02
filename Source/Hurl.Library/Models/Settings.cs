using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Hurl.Library.Models;

public partial class Settings : ObservableObject
{
    [JsonPropertyName("$schema")]
    public string Schema { get; } = "https://raw.githubusercontent.com/U-C-S/Hurl/main/Utils/UserSettings.schema.json";

    public string lastUpdated = DateTime.Now.ToString();

    [ObservableProperty]
    public partial ObservableCollection<Browser> Browsers { get; set; } = [];

    [ObservableProperty]
    public partial AppSettings AppSettings { get; set; } = new();

    [ObservableProperty]
    public partial List<Ruleset> Rulesets { get; set; } = [];
}
