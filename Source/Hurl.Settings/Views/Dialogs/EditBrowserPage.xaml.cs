using Hurl.Library.Models;
using Hurl.Settings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.IO;

namespace Hurl.Settings.Views.Dialogs;

public sealed partial class EditBrowserPage : Page
{
    public EditBrowserPageViewModel? ViewModel { get; private set; }

    public EditBrowserPage()
    {
        InitializeComponent();
    }

    public EditBrowserPage(Browser browser)
    {
        InitializeComponent();
        InitializeForBrowser(browser);
    }

    private void InitializeForBrowser(Browser browser)
    {
        var options = App.AppHost.Services.GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<Library.Models.Settings>>();
        var settingsService = App.AppHost.Services.GetRequiredService<Hurl.Settings.Services.Interfaces.ISettingsService>();

        ViewModel = new EditBrowserPageViewModel(browser, options, settingsService);
        this.DataContext = this;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is Browser browser)
        {
            InitializeForBrowser(browser);
        }
    }

    private void AddAlternate_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null) return;

        ViewModel.AddAlternate(AltNameInput.Text ?? string.Empty, AltArgsInput.Text ?? string.Empty);
        AltNameInput.Text = string.Empty;
        AltArgsInput.Text = string.Empty;
    }

    private void RemoveAlternate_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null) return;
        if (sender is Button btn && btn.DataContext is AlternateLaunch alt)
        {
            ViewModel.RemoveAlternate(alt);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null) return;
        ViewModel.Save();

        // Try to navigate back if possible
        if (Frame != null && Frame.CanGoBack)
            Frame.GoBack();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
            ViewModel.Revert();

        if (Frame != null && Frame.CanGoBack)
            Frame.GoBack();
    }

    private void OpenContainingExe_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null) return;

        try
        {
            var path = ViewModel.ExePath ?? string.Empty;
            if (File.Exists(path))
            {
                Process.Start("explorer", $"/select,\"{path}\"");
            }
            else
            {
                Process.Start("explorer");
            }
        }
        catch (Exception)
        {
            // ignore failurrs
        }
    }

    private void OpenContainingIcon_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null) return;

        try
        {
            var path = ViewModel.CustomIconPath ?? string.Empty;
            if (File.Exists(path))
            {
                Process.Start("explorer", $"/select,\"{path}\"");
            }
            else
            {
                Process.Start("explorer");
            }
        }
        catch (Exception)
        {
            // ignore failures
        }
    }
}

