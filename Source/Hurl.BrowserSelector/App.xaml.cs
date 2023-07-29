using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Windows;
using SingleInstanceCore;
using System.Windows;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstance
    {
        private MainWindow _mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            bool isFirstInstance = this.InitializeAsFirstInstance("HurlTray");
            if (isFirstInstance)
            {
                var cliArgs = CliArgs.GatherInfo(e.Args, false);
                UriGlobal.Value = cliArgs.Url;

                _mainWindow = new();
                _mainWindow.Init(cliArgs);
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
                var cliArgs = CliArgs.GatherInfo(args, true);
                var IsTimedSet = TimedBrowserSelect.CheckAndLaunch(cliArgs.Url);

                if (!IsTimedSet)
                {
                    UriGlobal.Value = cliArgs.Url;
                    _mainWindow.Init(cliArgs);
                }

            });
        }

        protected override void OnExit(ExitEventArgs e) => SingleInstance.Cleanup();
    }
}