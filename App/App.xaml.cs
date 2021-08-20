using System.Windows;

namespace Hurl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] Arguments = e.Args;
            new SelectionWindow(Arguments).Show();
        }
    }
}
