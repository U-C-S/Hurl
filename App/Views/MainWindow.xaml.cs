using Hurl.Browser;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BList x = BList.InitalGetList();
            foreach (BrowserObject i in x)
            {
                if (i.Name != null)
                {
                    Button button = new()
                    {
                        Padding = new Thickness(5),
                        Content = i.Name,
                        Tag = i.ExePath,
                    };
                    button.Click += BroClick;
                    _ = stacky.Children.Add(button);
                }

            }

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                argss.Text = Environment.GetCommandLineArgs()[1];
            }
        }

        private void BroClick(object sender, RoutedEventArgs e)
        {
            string path = (sender as Button).Tag.ToString();
            _ = Process.Start(path, "https://github.com/U-C-S/Hurl");
        }
    }
}
