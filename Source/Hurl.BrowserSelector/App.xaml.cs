using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Services;
using Hurl.BrowserSelector.ViewModels;
using Hurl.BrowserSelector.Windows;
using Hurl.Library;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Windows;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow? _mainWindow;

        private const string MUTEX_NAME = "Hurl_Mutex_3721";

        private Mutex? _singleInstanceMutex;
        private NamedPipeUrlReceiver? _pipeReceiver;

        public static IHost? AppHost { get; private set; }

        public App()
        {
            Current.Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile(Constants.APP_SETTINGS_MAIN, false, true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<Library.Models.Settings>(context.Configuration);
                    services.AddSingleton<SettingsService>();
                    services.AddSingleton<CurrentUrlService>();
                    services.AddTransient<SelectorWindowViewModel>();
                })
                .Build();
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string ErrorMsgBuffer;
            string ErrorWndTitle;
            switch (e.Exception?.InnerException)
            {
                case JsonException:
                    ErrorMsgBuffer = "The UserSettings.json file is in invalid JSON format. \n";
                    ErrorWndTitle = "Hurl - Invalid JSON";
                    break;
                default:
                    ErrorMsgBuffer = "An unknown error has occurred. \n";
                    ErrorWndTitle = "Hurl - Unknown Error";
                    break;

            }
            string errorMessage = string.Format("{0}\n{1}\n\n{2}", ErrorMsgBuffer, e.Exception?.InnerException?.Message, e.Exception?.Message);
            MessageBox.Show(errorMessage, ErrorWndTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _singleInstanceMutex = new Mutex(true, MUTEX_NAME, out var isOwned);

            if (!isOwned)
            {
                // Another instance is running - it will receive the URL via pipe from the Launcher
                Shutdown();
                return;
            }

            // Start the pipe receiver (always ready for connections with overlapping listeners)
            _pipeReceiver = new NamedPipeUrlReceiver(OnInstanceInvoked);
            _pipeReceiver.Start();

            var cliArgs = CliArgs.GatherInfo(e.Args, false);
            AppHost?.Services.GetRequiredService<CurrentUrlService>().Set(cliArgs.Url);

            _mainWindow = new();
            _mainWindow.Init(cliArgs);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_pipeReceiver != null)
            {
                _pipeReceiver.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }

            _singleInstanceMutex?.Close();

            base.OnExit(e);
        }

        private void OnInstanceInvoked(string[] args)
        {
            Current.Dispatcher.InvokeAsync(() =>
            {
                var cliArgs = CliArgs.GatherInfo(args, true);
                var IsTimedSet = TimedBrowserSelect.CheckAndLaunch(cliArgs.Url);

                if (!IsTimedSet)
                {
                    Debug.WriteLine($"Hurl Browser Selector: Instance Invoked with URL: {cliArgs.Url}");
                    AppHost?.Services.GetRequiredService<CurrentUrlService>().Set(cliArgs.Url);
                    _mainWindow?.Init(cliArgs);
                }
            });
        }
    }
}
