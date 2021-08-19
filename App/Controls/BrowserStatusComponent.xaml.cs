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

    }
}
