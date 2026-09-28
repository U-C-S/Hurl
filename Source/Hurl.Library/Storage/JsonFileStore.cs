using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Hurl.Library.Storage;

public sealed class JsonFileStore<T> where T : class
{
    private readonly string filePath;
    private readonly JsonTypeInfo<T> typeInfo;

    public JsonFileStore(string filePath, JsonTypeInfo<T> typeInfo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(typeInfo);
        this.filePath = Path.GetFullPath(filePath);
        this.typeInfo = typeInfo;
    }

    public bool Exists => File.Exists(filePath);

    public T? Read()
    {
        try
        {
            return JsonSerializer.Deserialize(File.ReadAllText(filePath), typeInfo);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        catch (DirectoryNotFoundException)
        {
            return null;
        }
    }

    public void Write(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        string json = JsonSerializer.Serialize(value, typeInfo);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, json);
    }
}
