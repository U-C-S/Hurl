using System.Windows;

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

        private void CancelClick(object sender, RoutedEventArgs e) => DialogResult = false;

        private void SaveButton(object sender, RoutedEventArgs e) => DialogResult = true;

        private void RemoveBtn(object sender, RoutedEventArgs e)
        {
            this.BrowserName = this.BrowserPath = "";
            DialogResult = true;
        }
    }
}
