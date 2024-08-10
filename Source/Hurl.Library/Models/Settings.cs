using System.Text.Json.Serialization;

namespace Hurl.Library.Models;

public class Settings
{
    [JsonPropertyName("$schema")]
    public string Schema { get; } = "https://raw.githubusercontent.com/U-C-S/Hurl/main/Utils/UserSettings.schema.json";

    //public string Version = Constants.VERSION;

    public string LastUpdated { get; set; } = DateTime.Now.ToString();

    public List<Browser> Browsers { get; set; } = [];
        
    public AppSettings AppSettings { get; set; } = new AppSettings();

    public List<Ruleset> Rulesets { get; set; } = [];
}
