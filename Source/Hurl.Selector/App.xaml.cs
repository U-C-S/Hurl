using Hurl.Selector.Pages;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Windows;

namespace Hurl.Selector;

public partial class App : Microsoft.UI.Xaml.Application
{
    public App()
    {
        this.InitializeComponent();
        Current.UnhandledException += Dispatcher_UnhandledException;
    }

    private MainWindow? _mainWindow;

    private readonly CancellationTokenSource _cancelTokenSource = new();
    private Thread? _pipeServerListenThread;
    private NamedPipeServerStream? _pipeserver;

    private void Dispatcher_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
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

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        //_pipeServerListenThread = new Thread(PipeServer);
        //_pipeServerListenThread.Start();

        //var cliArgs = CliArgs.GatherInfo(args.Arguments, false);
        //OpenedUri.Value = cliArgs.Url;

        _mainWindow = new();
        _mainWindow.GetWindow.Content = new SelectorPage();
        //_mainWindow.Init(cliArgs);
        _mainWindow.GetWindow.Activate();

        //m_window = new MainWindow();
        //m_window.Activate();
    }

    //protected override void Exit()
    //{
    //    _cancelTokenSource.Cancel();
    //    _pipeServerListenThread?.Join();

    //    _singleInstanceMutex?.Close();
    //    _singleInstanceWaitHandle?.Close();
    //    _pipeserver?.Dispose();

    //    base.OnExit(e);
    //}

    //public void OnInstanceInvoked(string[] args)
    //{
    //    var cliArgs = CliArgs.GatherInfo(args, true);
    //    var IsTimedSet = TimedBrowserSelect.CheckAndLaunch(cliArgs.Url);

    //    if (!IsTimedSet)
    //    {
    //        OpenedUri.Value = cliArgs.Url;
    //        _mainWindow?.Init(cliArgs);
    //    }
    //}

    //public void PipeServer()
    //{
    //    while (!_cancelTokenSource.Token.IsCancellationRequested)
    //    {
    //        _pipeserver = new("HurlNamedPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

    //        try
    //        {
    //            _pipeserver.WaitForConnectionAsync(_cancelTokenSource.Token).Wait();

    //            using StreamReader sr = new(_pipeserver);
    //            string args = sr.ReadToEnd();
    //            string[] argsArray = JsonSerializer.Deserialize<string[]>(args) ?? [];
    //            OnInstanceInvoked(argsArray);
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            break;
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.WriteLine($"Error in PipeServer: {e.Message}");
    //        }
    //        finally
    //        {
    //            _pipeserver.Dispose();
    //        }
    //    }
    //}
}


