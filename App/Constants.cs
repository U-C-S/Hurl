using System;
using System.IO;

namespace Hurl.BrowserSelector
{
    public class Constants
    {
        public const string NAME = "Hurl";
        public const string DESCRIPTION = "Hurl - A tool to select the browsers dynamically";
        public const string URLAssociations = "HandleURL3721";
        public const string VERSION = "0.7.1";

        public static string APP_PARENT_DIR = Environment.CurrentDirectory;
        public static string APP_LAUNCH_PATH = Environment.GetCommandLineArgs()[0];
        public static string ROAMING = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string SettingsFilePath = Path.Combine(ROAMING, "Hurl", "UserSettings.json");
        public static string APP_SETTINGS_DIR = Path.Combine(ROAMING, "Hurl");

        public const string NEW_LINE = "1&#x0a;";

    }
}
