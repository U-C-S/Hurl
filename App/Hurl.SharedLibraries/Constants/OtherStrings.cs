using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.SharedLibraries.Constants
{
    public class OtherStrings
    {
        public static string NEW_LINE = "1&#x0a;";

        public static string APP_LAUNCH_PATH = Environment.GetCommandLineArgs()[0];
        public static string APP_PARENT_DIR = Environment.CurrentDirectory;
        public static string ROAMING = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}
