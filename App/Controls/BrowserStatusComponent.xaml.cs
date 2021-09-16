using Hurl.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hurl.Controls
{
    /// <summary>
    /// Interaction logic for BrowserStatusComponent.xaml
    /// </summary>
    public partial class BrowserStatusComponent : UserControl
    {
        public BrowserStatusComponent()
        {
            InitializeComponent();

            DataContext = this;
        }

        public string BrowserName { get; set; }
        public string BrowserPath { get; set; }
        public bool EditEnabled { get; set; } = true;
        public string BackColor { get; set; } = "Black";
        public BitmapSource Img { get; set; }
        //public RoutedEventHandler DeleteItem;

        private void CopyPath(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(BrowserPath);
        }

        private void OpenExe(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(BrowserPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void EditTheBrowser(object sender, RoutedEventArgs e)
        {
            BrowserForm x = new BrowserForm()
            {
                BrowserName = BrowserName,
                BrowserPath = BrowserPath,
            };
            if (x.ShowDialog() == true)
            {
                string name = x.BrowserName;
                string path = x.BrowserPath;
                if (name.Equals("") && path.Equals(""))
                {
                    ((StackPanel)this.Parent).Children.Remove(this);
                }
                else
                {
                    this.BrowserName = name;
                    this.BrowserPath = path;
                    BrowserNameTextBlock.Text = BrowserName;
                }
            }
        }
    }
}
