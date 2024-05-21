using Hurl.Library.Extensions;
using Hurl.RulesetManager.Controls;
using Hurl.RulesetManager.ViewModels;
using System.Windows;
using Wpf.Ui.Appearance;

namespace Hurl.RulesetManager.Windows;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        IsVisibleChanged += MainWindow_IsVisibleChanged;
        SystemThemeWatcher.Watch(this);
    }

    private async void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        await RefreshAsync();
    }

    public Task RefreshAsync()
    {
        _rulesetsList.Children.Clear();

        foreach (var (ruleset, i) in SettingsState.Rulesets.WithIndex())
        {
            var accordion = new RulesetAccordion(i, ruleset) { Refresh = RefreshAsync };
            _rulesetsList.Children.Add(accordion);
        }

        return Task.CompletedTask;
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new EditRuleset(CreateRuleset);
        window.ShowDialog();
    }

    private void TestButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new RuleTestingWindow();
        window.ShowDialog();
    }

    public void CreateRuleset(EditRulesetViewModel vm)
    {
        SettingsState.Rulesets.Add(vm.ToRuleSet());
        SettingsState.Update();
        RefreshAsync();
    }
}
