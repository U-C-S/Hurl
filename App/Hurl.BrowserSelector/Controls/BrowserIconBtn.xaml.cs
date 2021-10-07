using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Hurl.BrowserSelector.Controls
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
