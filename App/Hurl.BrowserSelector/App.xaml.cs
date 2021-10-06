using System.Diagnostics;
using System.Windows;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] Arguments = e.Args;

            if (Arguments.Length == 0)
            {
                string[] Args = { "https://github.com" };
                MainWindow window = new MainWindow(Args);
                window.Show();
            }
            else
            {
                //var x = Arguments[0].Substring(7);
                //Process.Start("\"C:\\Program Files\\Firefox Developer Edition\\firefox.exe\"");
                MainWindow window = new MainWindow(Arguments);
                window.Show();
            }
        }
    }
}