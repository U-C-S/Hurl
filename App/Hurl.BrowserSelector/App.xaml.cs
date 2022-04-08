using Hurl.SharedLibraries.Constants;
using SingleInstanceCore;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstance
    {
        private MainWindow _mainWindow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isFirstInstance = this.InitializeAsFirstInstance("HurlTray");
            if (isFirstInstance)
            {
                _mainWindow = new MainWindow();
                var x = ArgProcess(e.Args, false);
                _mainWindow.Init(x);
            }
            else
            {
                Current.Shutdown();
            }
        }

        public void OnInstanceInvoked(string[] args)
        {
            Current.Dispatcher.Invoke(() =>
            {
                _mainWindow.Init(ArgProcess(args, true));
            });
        }

        private void Application_Exit(object sender, ExitEventArgs e) => SingleInstance.Cleanup();

        private static CLIArgs ArgProcess(string[] Args, bool SecondInstanceArgs)
        {
#if DEBUG
            Task.Run(() =>
            {
                var ArgsStoreFile = Path.Combine(OtherStrings.ROAMING, "Hurl", "args.txt");
                var StrFormat = $"\n\n{SecondInstanceArgs} -- {Args.Length} --- {string.Join(" ", Args)}";
                File.AppendAllText(ArgsStoreFile, StrFormat);
            });
#endif

            CLIArgs cliargs = new();

            var ArgsLength = Args.Length;
            if (ArgsLength > 0)
            {
                string whatever = Args[0];

                if (SecondInstanceArgs)
                {
                    cliargs.IsSecondInstance = true;
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

    public record CLIArgs
    {
        public bool IsSecondInstance = false;
        public bool IsRunAsMin = false;
        public bool IsProtocolActivated = false;
        public string Url { get; set; } = string.Empty;
        public string[] otherArgs;
    }
}