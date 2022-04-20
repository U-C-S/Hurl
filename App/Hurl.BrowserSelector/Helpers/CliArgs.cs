using Hurl.SharedLibraries.Constants;
using System.IO;
using System.Threading.Tasks;

namespace Hurl.BrowserSelector.Helpers
{
    public record CliArgs
    {
        public bool IsSecondInstance = false;
        public bool IsRunAsMin = false;
        public bool IsProtocolActivated = false;
        public string Url { get; set; } = string.Empty;
        public string[] otherArgs;
    }

    public static class CliArgsMethods
    {
        public static CliArgs ArgProcess(string[] Args, bool SecondInstanceArgs)
        {
#if DEBUG
            Task.Run(() =>
            {
                var ArgsStoreFile = Path.Combine(OtherStrings.ROAMING, "Hurl", "args.txt");
                var StrFormat = $"\n\n{SecondInstanceArgs} --- {Args.Length} - {string.Join("__", Args)}";
                File.AppendAllText(ArgsStoreFile, StrFormat);
            });
#endif

            CliArgs cliargs = new();

            var ArgsLength = Args.Length;
            if (ArgsLength > 0)
            {
                string whatever = Args[0];

                if (SecondInstanceArgs)
                {
                    cliargs.IsSecondInstance = true;
                    if(ArgsLength >= 2)
                        whatever = Args[1];
                }

                if (whatever.StartsWith("hurl://"))
                {
                    cliargs.IsProtocolActivated = true;
                    cliargs.Url = whatever.Substring(7);
                }
                //else if(whatever.StartsWith("https://" || "http://"))
                else
                {
                    cliargs.otherArgs = Args.Length > 2 ? Args[2..] : null;
                    cliargs.Url = whatever;
                }
            }

            return cliargs;
        }
    }
}
