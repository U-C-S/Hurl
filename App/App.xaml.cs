using Hurl.Views;
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
                SelectionWindow window = new SelectionWindow(Arguments);
                window.Show();
            }
            else
            {
                NoArgsWindow window = new NoArgsWindow();
                window.Show();
            }

        }
    }
}
