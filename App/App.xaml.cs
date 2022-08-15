using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Views;
using Hurl.BrowserSelector.Views.ViewModels;
using SingleInstanceCore;
using System.Diagnostics;
using System.Windows;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstance
    {
        private MainWindow _mainWindow;
        private MainViewModel viewModel;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isFirstInstance = this.InitializeAsFirstInstance("HurlTray");
            if (isFirstInstance)
            {
                var x = CliArgs.GatherInfo(e.Args, false);
                CurrentLink.Value = x.Url;
                viewModel = new MainViewModel();

                _mainWindow = new MainWindow()
                {
                    DataContext = viewModel
                };

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
                var x = CliArgs.GatherInfo(args, true);
                CurrentLink.Value = x.Url;
                _mainWindow.Init(x);
            });
        }

        private void Application_Exit(object sender, ExitEventArgs e) => SingleInstance.Cleanup();
    }
}