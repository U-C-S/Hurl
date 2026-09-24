using System.Text.Json.Serialization;

namespace Hurl.Library.Models;

/// <summary>Icon overrides. A null Browser.Icon uses the executable's first icon.</summary>
public sealed record BrowserIcon
{
    public BrowserIconSource Source { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Path { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Index { get; init; }
}

[JsonConverter(typeof(JsonStringEnumConverter<BrowserIconSource>))]
public enum BrowserIconSource
{
    Executable,
    LocalImage,
    Url
}