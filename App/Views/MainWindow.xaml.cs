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
        private string? arg = null;

        public MainWindow(string[] x)
        {
            InitializeComponent();
            if (x.Length >= 1)
            {
                arg = argss.Text = x[0];
            }

            //What does \"%1\" mean in Registry ? 
            //https://www.tek-tips.com/viewthread.cfm?qid=382878
            //Environment.GetCommandLineArgs()[0] + 
        }

        public MainWindow()
        {
            InitializeComponent();
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
                        Margin = new Thickness(2),
                        Content = i.Name,
                        Tag = i.ExePath,
                        //Style = (Style) FindResource("MDIXButton")
                    };
                     
                    button.Click += BroClick;
                    _ = stacky.Children.Add(button);
                }

            }
        }

        private void BroClick(object sender, RoutedEventArgs e)
        {
            if (sender != null)
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


/*
//TODO
- https://stackoverflow.com/questions/11483655/icon-inside-of-button/11483844
*/