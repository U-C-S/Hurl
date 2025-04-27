using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.State;
using Hurl.BrowserSelector.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
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

        public App()
        {
            Current.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
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
            OpenedUri.Value = cliArgs.Url;

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
                    OpenedUri.Value = cliArgs.Url;
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

                    using StreamReader sr = new(_pipeserver);
                    string args = sr.ReadToEnd();
                    string[] argsArray = JsonSerializer.Deserialize<string[]>(args) ?? [];
                    OnInstanceInvoked(argsArray);
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
    }
}
