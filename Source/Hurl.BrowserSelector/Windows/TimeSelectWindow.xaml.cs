using Hurl.BrowserSelector.Helpers;
using Hurl.Library.Models;
using System.Collections.Generic;
using Wpf.Ui.Controls;

namespace Hurl.BrowserSelector.Windows
{
    /// <summary>
    /// Interaction logic for TimeSelectWindow.xaml
    /// </summary>
    public partial class TimeSelectWindow : FluentWindow
    {
        private List<Browser> browsers;

        public TimeSelectWindow(List<Browser> x)
        {
            InitializeComponent();
            browsers = x;
            DataContext = x.ConvertAll(y => y.Name);
        }

        private void SetBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var x = TimeBox.Value;
            var y = BrowserBox.SelectedIndex;

            if (y != -1)
            {
                TimedBrowserSelect.Create((int)x, browsers[y]);

                this.Close();
            }
        }

        private void CancelBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TimedBrowserSelect.DeleteCurrent();
            this.Close();
        }
    }
}
