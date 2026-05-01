using Hurl.Library.Models;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Hurl.Selector.Serialization;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    WriteIndented = true)]
[JsonSerializable(typeof(Settings))]
[JsonSerializable(typeof(AppSettings))]
[JsonSerializable(typeof(Browser))]
[JsonSerializable(typeof(AlternateLaunch))]
[JsonSerializable(typeof(ObservableCollection<Browser>))]
internal partial class SelectorJsonSerializerContext : JsonSerializerContext
{
}
