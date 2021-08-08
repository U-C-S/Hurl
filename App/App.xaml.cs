using Hurl.Views;
using System;
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

            if (Arguments.Length != 0)
            {
                var window = new SelectionWindow(Arguments);
                window.Show();
            }
            else
            {
                var window = new NoArgsWindow();
                window.Show();
            }

        }
    }
}
