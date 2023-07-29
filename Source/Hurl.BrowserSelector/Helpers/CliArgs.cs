using Hurl.Library;
using System.IO;
using System.Threading.Tasks;

namespace Hurl.BrowserSelector.Helpers
{
    public class CliArgs
    {
        public bool IsSecondInstance = false;
        public bool IsRunAsMin = false;
        public bool IsProtocolActivated = false;
        public string Url { get; set; } = string.Empty;
        public string[] otherArgs;

        private CliArgs(string[] Args, bool IsSecondInstance)
        {
            var ArgsLength = Args.Length;
            if (ArgsLength == 0) return;

            if (Args[0] == "--minimized")
            {
                IsRunAsMin = true;
                return;
            }

            if (IsSecondInstance)
            {
                this.IsSecondInstance = true;
                if (ArgsLength >= 2) Args = Args[1..];
            }

            if (Args[0].StartsWith("hurl://"))
            {
                IsProtocolActivated = true;
                Url = Args[0][7..];
            }
            else
            {
                otherArgs = Args.Length > 2 ? Args[2..] : null;
                Url = Args[0].Contains(' ') ? $"\"{Args[0]}\"" : Args[0];
            }
        }

        public static CliArgs GatherInfo(string[] Args, bool isSecondInstance)
        {
#if DEBUG
            Task.Run(() =>
            {
                Directory.CreateDirectory(Constants.APP_SETTINGS_DIR);
                var ArgsStoreFile = Path.Combine(Constants.ROAMING, "Hurl", "args.txt");
                var StrFormat = $"\n\n{isSecondInstance} --- {Args.Length} - {string.Join("__", Args)}";
                File.AppendAllText(ArgsStoreFile, StrFormat);
            });
#endif

            return new CliArgs(Args, isSecondInstance);
        }

    }
}
