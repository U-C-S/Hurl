using SingleInstanceCore;
using System.Windows;

namespace Hurl.BrowserSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstance
    {
        private MainWindow _mainWindow;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isFirstInstance = this.InitializeAsFirstInstance("HurlTray");
            if (!isFirstInstance)
            {
                Current.Shutdown();
            }
            else
            {
                _mainWindow = new MainWindow();
                _mainWindow.Show();
            }
        }

        public void OnInstanceInvoked(string[] args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _mainWindow.Activate();
                _mainWindow.WindowState = WindowState.Normal;

                // Invert the topmost state twice to bring the window on
                // top if it wasnt previously or do nothing
                _mainWindow.Topmost = !MainWindow.Topmost;
                _mainWindow.Topmost = !MainWindow.Topmost;
            });
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SingleInstance.Cleanup();
        }

        private string ArgProcess(string[] Args)
        {
            if (Args.Length > 0)
            {
                string link = Args[0];
                if (link.StartsWith("hurl://"))
                {
                    return link.Substring(7);
                }
                else
                {
                    return link;
                }
            }

            return null;
        }
    }
}