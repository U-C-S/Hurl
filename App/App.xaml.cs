using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Models;
using Hurl.BrowserSelector.Views;
using Hurl.BrowserSelector.Views.ViewModels;
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
        private MainViewModel viewModel;
        private readonly Settings settings = SettingsFile.GetSettings();

        protected override void OnStartup(StartupEventArgs e)
        {
            bool isFirstInstance = this.InitializeAsFirstInstance("HurlTray");
            if (isFirstInstance)
            {
                var x = CliArgs.GatherInfo(e.Args, false);
                //var IsTimedSet = TimedBrowserSelect.CheckAndLaunch(x.Url);

                CurrentLink.Value = x.Url;
                viewModel = new MainViewModel(settings);

                _mainWindow = new MainWindow(settings)
                {
                    DataContext = viewModel,
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
                var IsTimedSet = TimedBrowserSelect.CheckAndLaunch(x.Url);

                CurrentLink.Value = x.Url;
                _mainWindow.Init(x, IsTimedSet);
            });
        }

        protected override void OnExit(ExitEventArgs e) => SingleInstance.Cleanup();
    }
}