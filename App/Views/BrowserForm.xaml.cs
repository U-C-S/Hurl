using System;
using System.Windows;
using System.Windows.Forms;

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

        private void TextBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Select the Path of the EXE",
                Filter = "Application File (*.exe)|*.exe",
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExePathBox.Text = dialog.FileName;
            }
        }
    }
}
