using System;
using System.IO;

namespace Hurl.Library
{
    public class Constants
    {
        public const string NAME = "Hurl";
        public const string DESCRIPTION = "Hurl - A tool to select the browsers dynamically";
        public const string URLAssociations = "HandleURL3721";
        public const string VERSION = "0.8.0";

        public static string APP_PARENT_DIR = Environment.CurrentDirectory;
        public static string APP_LAUNCH_PATH = Environment.GetCommandLineArgs()[0];
        public static string ROAMING = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string APP_SETTINGS_DIR = Path.Combine(ROAMING, "Hurl");
        public static string APP_SETTINGS_MAIN = Path.Combine(APP_SETTINGS_DIR, "UserSettings.json");

        public const string NEW_LINE = "1&#x0a;";

#if DEBUG
        public const string SETTINGS_APP_PATH = "..\\..\\..\\..\\Hurl.SettingsApp\\bin\\x64\\Debug\\net7.0-windows10.0.22000.0\\win10-x64\\Hurl.SettingsApp.exe";
#else
        public const string SETTINGS_APP_PATH = ".\\Hurl.SettingsApp.exe";
#endif

    }
}
