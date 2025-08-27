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
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
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
        private const string EVENT_NAME = "Hurl_Event_3721";

        private Mutex? _singleInstanceMutex;
        private EventWaitHandle? _singleInstanceWaitHandle;

        private readonly CancellationTokenSource _cancelTokenSource = new();
        private Thread? _pipeServerListenThread;
        public static IHost AppHost { get; } = InitAppHost();

        public App()
        {
            Current.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private static IHost InitAppHost()
        {
            return Host
                .CreateDefaultBuilder()
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
            _singleInstanceWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, EVENT_NAME);

            if (!isOwned)
            {
                _singleInstanceWaitHandle.Set();
                Shutdown();
                return;
            }

            new Thread(() =>
            {
                while (_singleInstanceWaitHandle.WaitOne())
                {
                    Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (Current.MainWindow is { } window)
                        {
                            _mainWindow?.ShowWindow();
                        }
                        else
                        {
                            Shutdown();
                        }
                    });
                }
            })
            {
                IsBackground = true
            }.Start();

            _pipeServerListenThread = new Thread(PipeServer);
            _pipeServerListenThread.Start();

            var cliArgs = CliArgs.GatherInfo(e.Args, false);
            //OpenedUri.Value = cliArgs.Url;
            AppHost.Services.GetRequiredService<CurrentUrlService>().Set(cliArgs.Url);

            _mainWindow = new();
            _mainWindow.Init(cliArgs);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _cancelTokenSource.Cancel();
            _pipeServerListenThread?.Join();

            _singleInstanceMutex?.Close();
            _singleInstanceWaitHandle?.Close();

            base.OnExit(e);
        }

        public void OnInstanceInvoked(string[] args)
        {
            Current.Dispatcher.InvokeAsync(() =>
            {
                var cliArgs = CliArgs.GatherInfo(args, true);
                var IsTimedSet = TimedBrowserSelect.CheckAndLaunch(cliArgs.Url);

                if (!IsTimedSet)
                {
                    //Debug.WriteLine($"Hurl Browser Selector: Instance Invoked with URL: {cliArgs.Url}");
                    AppHost.Services.GetRequiredService<CurrentUrlService>().Set(cliArgs.Url);
                    _mainWindow?.Init(cliArgs);
                }
            });
        }

        public void PipeServer()
        {
            PipeSecurity pipeSecurity = new();
            pipeSecurity.AddAccessRule(new PipeAccessRule(
                new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                PipeAccessRights.ReadWrite,
                AccessControlType.Allow));

            while (!_cancelTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    using var _pipeserver = NamedPipeServerStreamAcl.Create(
                        "HurlNamedPipe",
                        PipeDirection.InOut, 1,
                        PipeTransmissionMode.Byte,
                        PipeOptions.Asynchronous,
                        0, 0,
                        pipeSecurity);
                    _pipeserver.WaitForConnectionAsync(_cancelTokenSource.Token).Wait();

                    ProcessMessage(_pipeserver);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error in PipeServer: {e.Message}");
                }
            }
        }

        public void ProcessMessage(NamedPipeServerStream pipeserver)
        {
            try
            {
                var buffer = new byte[4096];
                int bytesRead = pipeserver.Read(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (message == "ping")
                {
                    using StreamWriter sw = new(pipeserver)
                    {
                        AutoFlush = true,
                    };
                    sw.Write("pong");

                    buffer = new byte[4096];
                    bytesRead = pipeserver.Read(buffer, 0, buffer.Length);
                    message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                }
                else if (string.IsNullOrEmpty(message))
                {
                    return;
                }

                string[] argsArray = JsonSerializer.Deserialize<string[]>(message) ?? [];
                OnInstanceInvoked(argsArray);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ProcessMessage: {ex.Message}");
            }
        }
    }
}
