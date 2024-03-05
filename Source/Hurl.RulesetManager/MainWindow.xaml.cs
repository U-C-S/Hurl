using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;

namespace Hurl.RulesetManager;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        //Loaded += (sender, args) => SystemThemeWatcher.Watch(this, Wpf.Ui.Controls.WindowBackdropType.Mica, true);
        DataContext = Hurl.Library.SettingsFile.GetSettings().Rulesets;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e) => this.Close();
}
