using System;
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
            var URL = ArgProcess(e.Args);
            new MainWindow(URL).Show();
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