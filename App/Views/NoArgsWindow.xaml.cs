using Hurl.Browser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                    TextBlock text = new()
                    {
                        Padding = new Thickness(2),
                        Text = $"- {i.Name}"
                    };
                    _ = stacky.Children.Add(text);
                }

            }
        }
    }
}
