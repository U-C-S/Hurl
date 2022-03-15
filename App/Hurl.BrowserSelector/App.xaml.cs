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
            if (!isFirstInstance)
            {
                Current.Shutdown();
            }
            else
            {
                _mainWindow = new MainWindow();
                _mainWindow.init(ArgProcess(e.Args, false));
            }
        }

        public void OnInstanceInvoked(string[] args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _mainWindow.init(ArgProcess(args, true));
                _mainWindow.WindowState = WindowState.Normal;
            });
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SingleInstance.Cleanup();
        }

        private string ArgProcess(string[] Args, bool SecondInstanceArgs)
        {
#if DEBUG
            Task.Run(() =>
            {
                var ArgsStoreFile = Path.Combine(OtherStrings.ROAMING, "Hurl", "args.txt");
                var StrFormat = $"\n\n{SecondInstanceArgs} -- {Args.Length} --- {string.Join(" ", Args)}";
                File.AppendAllText(ArgsStoreFile, StrFormat);
            });
#endif

            var ArgsLength = Args.Length;
            if (ArgsLength > 0)
            {
                string link = Args[0];
                if (SecondInstanceArgs)
                {
                    if (ArgsLength > 1)
                        link = Args[1];
                    else
                        return null;
                }

                if (link.StartsWith("hurl://"))
                {
                    return link.Substring(7);
                }
                else
                {
                    return link;
                }
            }

            return null;
        }
    }
}