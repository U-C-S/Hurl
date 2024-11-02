using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace Hurl.Settings;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
        this.UnhandledException += Dispatcher_UnhandledException;
    }

    private void Dispatcher_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
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
        string errorMessage = string.Format(
            "{0}\n{1}\n\n{2}\nStack Trace:\n\n{3}",
            ErrorMsgBuffer,
            e.Exception?.InnerException?.Message,
            e.Exception?.Message,
            e.Exception?.StackTrace);
        System.Windows.MessageBox.Show(errorMessage,
                                       ErrorWndTitle,
                                       System.Windows.MessageBoxButton.OK,
                                       System.Windows.MessageBoxImage.Error);
        Exit();
    }

    private MainWindow m_window;

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var cmdArgs = Environment.GetCommandLineArgs();

        m_window = new MainWindow();

        if (cmdArgs.Length > 1)
        {
            ProcessArgs(cmdArgs);
        }
        else
        {
            m_window.Activate();
        }
    }


    void ProcessArgs(string[] args)
    {
        Debug.WriteLine(args[0]);
        var primaryArg = args[1];

        if (primaryArg.Equals("--page"))
        {
            if (args.Length > 2)
            {
                //m_window.NavigateToPage(args[2]);
                m_window.Activate();
            }
        }
    }
}
