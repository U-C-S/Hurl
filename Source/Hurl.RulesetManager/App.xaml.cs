using Hurl.RulesetManager.Windows;
using System.Windows;

namespace Hurl.RulesetManager;

public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow mainWindow = new();
        mainWindow.Show();
    }
}
