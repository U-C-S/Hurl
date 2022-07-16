using Hurl.BrowserSelector.Helpers;
using SingleInstanceCore;
using System.Windows;
using Hurl.BrowserSelector.Views;

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
                _mainWindow.Init(CliArgs.GatherInfo(e.Args, false));
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
                _mainWindow.Init(CliArgs.GatherInfo(args, true));
            });
        }

        private void Application_Exit(object sender, ExitEventArgs e) => SingleInstance.Cleanup();
    }
}