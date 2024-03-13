using Hurl.RulesetManager.Windows;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Hurl.RulesetManager;


public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var window = new MainWindow();
        window.Show();
    }
}
