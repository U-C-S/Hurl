using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
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
        private MainWindow _mainWindow;

        private const string MUTEX_NAME = "Hurl_Mutex_3721";
        private const string EVENT_NAME = "Hurl_Event_3721";

        private Mutex? _singleInstanceMutex;
        private EventWaitHandle? _singleInstanceWaitHandle;

        private Thread _pipeServerListenThread;

        public App()
        {
            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string ErrorMsgBuffer;
            string ErrorWndTitle;
            switch (e.Exception?.InnerException)
            {
                case JsonException:
                    ErrorMsgBuffer = "The UserSettings.json file is in invalid JSON format. \n";
                    ErrorWndTitle = "Invalid JSON";
                    break;
                default:
                    ErrorMsgBuffer = "An unknown error has occurred. \n";
                    ErrorWndTitle = "Unknown Error";
                    break;

            }
            string errorMessage = string.Format("{0}\n{1}\n\n{2}", ErrorMsgBuffer, e.Exception?.InnerException?.Message, e.Exception.Message);
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
                    Current.Dispatcher.BeginInvoke(async () =>
                    {
                        if (Current.MainWindow is { } window)
                        {
                            _mainWindow.MaximizeWindow();
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
            UriGlobal.Value = cliArgs.Url;

            _mainWindow = new();
            _mainWindow.Init(cliArgs);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _singleInstanceMutex?.Close();

            // Also kill the pipe server thread
            // A temporary solution, ideally the pipe server should be stopped gracefully
            Process.GetCurrentProcess().Kill();
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

        public void PipeServer()
        {
            while (true)
            {
                using NamedPipeServerStream pipeserver = new("HurlNamedPipe", PipeDirection.InOut);

                pipeserver.WaitForConnection();

                try
                {
                    using (StreamReader sr = new(pipeserver))
                    {
                        string args = sr.ReadToEnd();
                        string[] argsArray = JsonSerializer.Deserialize<string[]>(args) ?? [];
                        OnInstanceInvoked(argsArray);
                    }
                }
                catch (Exception _) { }

                pipeserver.Close();
            }
        }
    }
}
