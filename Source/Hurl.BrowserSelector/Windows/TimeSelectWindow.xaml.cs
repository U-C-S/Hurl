using Hurl.BrowserSelector.Helpers;
using Hurl.Library.Models;
using System.Collections.Generic;

namespace Hurl.BrowserSelector.Windows
{
    /// <summary>
    /// Interaction logic for TimeSelectWindow.xaml
    /// </summary>
    public partial class TimeSelectWindow : Wpf.Ui.Controls.UiWindow
    {
        private List<Browser> browsers;

        public TimeSelectWindow(List<Browser> x)
        {
            InitializeComponent();
            browsers = x;
            DataContext = x.ConvertAll(y => y.Name);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var x = TimeBox.Value;
            var y = BrowserBox.SelectedIndex;

            if (y != -1)
            {
                TimedBrowserSelect.Create((int)x, browsers[y]);

                this.Close();
            }
        }
    }
}
