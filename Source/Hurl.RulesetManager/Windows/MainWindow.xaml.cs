﻿using Hurl.Library.Extensions;
using Hurl.Library.Models;
using Hurl.RulesetManager.Controls;
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

namespace Hurl.RulesetManager.Windows;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        IsVisibleChanged += MainWindow_IsVisibleChanged;
    }

    private async void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        await RefreshAsync();
    }

    private Task RefreshAsync()
    {
        _rulesetsList.Children.Clear();

        foreach (var (ruleset, i) in Hurl.Library.SettingsFile.GetSettings().Rulesets.WithIndex())
        {
            var accordion = new RulesetAccordion(i, ruleset);
            _rulesetsList.Children.Add(accordion);
        }

        return Task.CompletedTask;
    }
}
