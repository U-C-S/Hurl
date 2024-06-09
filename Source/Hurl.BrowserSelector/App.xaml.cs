using Hurl.BrowserSelector.Globals;
using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Windows;
using System.Text.Json;
using System.Windows;
using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.IO;
using System.Threading;


namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _mainWindow;

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
            Thread thread = new Thread(PipeServer);
            thread.Start();

            var cliArgs = CliArgs.GatherInfo(e.Args, false);
            UriGlobal.Value = cliArgs.Url;

            _mainWindow = new();
            _mainWindow.Init(cliArgs);
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
                using NamedPipeServerStream pipeserver = new("HurlNamedPipe", PipeDirection.InOut, 3);

                pipeserver.WaitForConnection();
                using (StreamReader sr = new(pipeserver))
                {
                    string args = sr.ReadToEnd();
                    string[] argsArray = JsonSerializer.Deserialize<string[]>(args);
                    //Debug.WriteLine(argsArray);
                    OnInstanceInvoked(argsArray);
                }

                pipeserver.Close();
            }
        }
    }
}
