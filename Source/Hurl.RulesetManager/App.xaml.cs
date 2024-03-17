using Hurl.RulesetManager.Windows;
using System.Text.Json;
using System.Windows;

namespace Hurl.RulesetManager;

public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow mainWindow = new();
        mainWindow.Show();
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
}
