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
            string[] x = e.Args;
            //Console.WriteLine(x.Length);
            MainWindow window = new MainWindow(x);
            window.Show();

        }
    }
}
