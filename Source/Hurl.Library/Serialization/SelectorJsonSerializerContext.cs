using Hurl.Library.Models;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Hurl.Library.Serialization;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    WriteIndented = true)]
[JsonSerializable(typeof(Settings))]
[JsonSerializable(typeof(AppSettings))]
[JsonSerializable(typeof(Browser))]
[JsonSerializable(typeof(AlternateLaunch))]
[JsonSerializable(typeof(Ruleset))]
[JsonSerializable(typeof(ObservableCollection<Browser>))]
public partial class SelectorJsonSerializerContext : JsonSerializerContext { }
