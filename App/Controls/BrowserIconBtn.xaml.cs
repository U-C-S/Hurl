using Hurl.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hurl.Controls
{
    /// <summary>
    /// Interaction logic for BrowserIconBtn.xaml
    /// </summary>
    public partial class BrowserIconBtn : UserControl
    {
        public BrowserIconBtn()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string BrowserName { get; set; } = "null";
        public ImageSource BrowserIcon { get; set; }
        public string ExePath { get; set; }
        public string URL { get; set; }

        private void OpenIt(object sender, MouseButtonEventArgs e)
        {
            Process.Start(ExePath, URL);
        }
    }
}
