using System.IO;

namespace Hurl.SharedLibraries.Constants
{
    public class MetaStrings
    {
        public const string NAME = "Hurl";
        public const string DESCRIPTION = "Hurl - A tool to select the browsers dynamically";
        public const string URLAssociations = "HandleURL3721";
        public const string VERSION = "0.6.0";
        public static string SettingsFilePath = Path.Combine(OtherStrings.ROAMING, "Hurl", "UserSettings.json");
    }
}