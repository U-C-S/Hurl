using Hurl.Library.Models;
using Hurl.RulesetManager.ViewModels;
using Hurl.RulesetManager.Windows;
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

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var id = (int)((Button)sender).Tag;
        var ruleset = ((List<Ruleset>)DataContext).Find(x => x.Id == id);

        var ViewModel = new EditRulesetViewModel(ruleset);
        var window = new EditRuleset(ViewModel, UpdateRuleset) { Owner = Window.GetWindow(this) };
        window.ShowDialog();
    }

    public void UpdateRuleset(EditRulesetViewModel vm)
    {
        var ruleset = vm.ToRuleSet();
    }
}
