using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using Hurl.Browser;
using System;

namespace Hurl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BList x = BList.InitalGetList();
            foreach (BrowserObject i in x)
            {
                if (i.Name != null)
                {
                    Button button = new();
                    button.Content = i.Name;
                    button.Tag = i.ExePath;
                    button.Click += BroClick;
                    stacky.Children.Add(button);
                }

            }

            (sender as Button).IsEnabled = false;
            if(Environment.GetCommandLineArgs().Length > 1)
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
