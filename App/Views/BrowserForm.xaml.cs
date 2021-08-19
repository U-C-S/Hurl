using System;
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
    /// Interaction logic for BrowserForm.xaml
    /// </summary>
    public partial class BrowserForm : Window
    {
        public BrowserForm()
        {
            InitializeComponent();

            DataContext = this;
        }

        public string BrowserName { get; set; } = "";
        public string BrowserPath { get; set; } = "";
    }
}
