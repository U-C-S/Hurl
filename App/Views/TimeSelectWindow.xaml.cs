using System.Windows.Input;

namespace Hurl.BrowserSelector.Views
{
    /// <summary>
    /// Interaction logic for TimeSelectWindow.xaml
    /// </summary>
    public partial class TimeSelectWindow : Wpf.Ui.Controls.UiWindow
    {
        public TimeSelectWindow()
        {
            InitializeComponent();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => this.DragMove();
    }
}
