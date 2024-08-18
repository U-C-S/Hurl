using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hurl.Library;

public class JsonOperations
{
    public static Model? FromJsonToModel<Model>(string pathToJSON)
    {
        string jsondata = File.ReadAllText(pathToJSON);
        var SettingsObject = JsonSerializer.Deserialize<Model>(jsondata);
        return SettingsObject;
    }

    public static void FromModelToJson(object Obj, string pathToJSON)
    {
        var x = JsonSerializer.Serialize(Obj, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        File.WriteAllText(pathToJSON, x);
    }
}
