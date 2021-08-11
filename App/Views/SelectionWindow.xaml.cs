using Hurl.Browser;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hurl.Views
{
    /// <summary>
    /// Interaction logic for SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        private string arg = null;

        public SelectionWindow(string[] x)
        {
            InitializeComponent();
            PreviewKeyDown += new KeyEventHandler(Window_Esc);

            if (x.Length >= 1)
            {
                arg = argss.Text = x[0];
            }

            //What does \"%1\" mean in Registry ? 
            //https://www.tek-tips.com/viewthread.cfm?qid=382878
            //Environment.GetCommandLineArgs()[0] + 
        }

        private void Window_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Window_Deactivated(object sender, EventArgs e) => Close();


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BList x = BList.InitalGetList();
            foreach (BrowserObject i in x)
            {
                if (i.Name != null)
                {
                    Button button = new Button()
                    {
                        Padding = new Thickness(5),
                        Margin = new Thickness(20, 5, 20, 0),
                        Content = i.Name,
                        Tag = i.ExePath,
                        ContextMenu = FindResource("extraOptions") as ContextMenu
                        //Style = (Style) FindResource("MDIXButton")
                    };

                    button.Click += BroClick;
                    stacky.Children.Add(button);
                    //stacky.Children.Add(button);
                }

            }
        }

        private void BroClick(object sender, RoutedEventArgs e)
        {
            if (arg != null)
            {
                string path = (sender as Button).Tag.ToString();
                _ = Process.Start(path, arg);
            }
        }

        private void Incognito_Click(object sender, RoutedEventArgs e)
        {
            if (arg != null)
            {
                MenuItem menuItem = e.Source as MenuItem;
                ContextMenu parent = menuItem.Parent as ContextMenu;
                Button SrcButton = parent.PlacementTarget as Button;

                string path = SrcButton.Tag.ToString();
                string theArgs = $"--incognito {arg}";
                _ = Process.Start(path, theArgs);
            }
            else
            {
                MessageBox.Show("No link to open");
            }
        }
    }
}
