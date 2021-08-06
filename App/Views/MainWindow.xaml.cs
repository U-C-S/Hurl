using Hurl.Browser;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string arg;

        public MainWindow()
        {
            InitializeComponent();
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                arg = argss.Text = Environment.GetCommandLineArgs()[1];
            }
        }

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
        }

        private void BroClick(object sender, RoutedEventArgs e)
        {
            if(sender != null)
            {
                string path = (sender as Button)!.Tag.ToString()!;
                if (arg != null)
                {
                    _ = Process.Start(path, arg);
                }
                else
                {
                    _ = Process.Start(path, "https://github.com/U-C-S/Hurl");
                }
            }

        }
    }
}
