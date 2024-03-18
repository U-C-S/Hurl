using System;
using System.IO;

namespace Hurl.Library;

public class Constants
{
    public const string NAME = "Hurl";
    public const string DESCRIPTION = "Hurl - A tool to select the browsers dynamically";
    public const string URLAssociations = "HandleURL3721";
    public const string VERSION = "0.8.2";

    public static string APP_PARENT_DIR = Environment.CurrentDirectory;
    public static string APP_LAUNCH_PATH = Environment.GetCommandLineArgs()[0];
    public static string ROAMING = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public static string APP_SETTINGS_DIR = Path.Combine(ROAMING, "Hurl");
    public static string APP_SETTINGS_MAIN = Path.Combine(APP_SETTINGS_DIR, "UserSettings.json");
#if DEBUG
    public static string SETTINGS_APP = Path.GetFullPath(Path.Combine(APP_PARENT_DIR, "../../../../Hurl.RulesetManager/bin/Debug/net8.0-windows/Hurl.RulesetManager.exe"));
#else
    public static string SETTINGS_APP = Path.Combine(APP_PARENT_DIR, "Hurl.RulesetManager.exe");
#endif

    public const string NEW_LINE = "1&#x0a;";
}
