using Hurl.Browser;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Hurl.Views
{
    /// <summary>
    /// Interaction logic for NoArgsWindow.xaml
    /// </summary>
    public partial class NoArgsWindow : Window
    {
        public NoArgsWindow()
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
                    TextBlock text = new TextBlock()
                    {
                        Padding = new Thickness(2),
                        Text = $"- {i.Name}"
                    };
                    _ = stacky.Children.Add(text);
                }

            }
            // https://stackoverflow.com/a/909859
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            //Application.Current.MainWindow.Width = 420;
            new SettingsWindow().ShowDialog();
        }
    }
}