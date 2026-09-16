using Hurl.Library.Models;
using Hurl.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Hurl.App.Views;

public sealed partial class EditBrowserPage : Page
{
    public EditBrowserPageViewModel? ViewModel { get; private set; }
    public ObservableCollection<string> BreadcrumbItems { get; } = ["Browsers"];

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
        var settingsService = App.Services!.GetRequiredService<Hurl.App.Services.Interfaces.ISettingsService>();

        BreadcrumbItems.Add(browser.Name);
        ViewModel = new EditBrowserPageViewModel(browser, settingsService);
        this.DataContext = this;
    }

    private void InitializeForNewBrowser()
    {
        var settingsService = App.Services!.GetRequiredService<Hurl.App.Services.Interfaces.ISettingsService>();

        BreadcrumbItems.Add("New Browser");
        ViewModel = new EditBrowserPageViewModel(new Browser(), settingsService, isNewBrowser: true);
        DataContext = this;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (ViewModel != null)
        {
            return;
        }

        if (e.Parameter is Browser browser)
        {
            InitializeForBrowser(browser);
        }
        else
        {
            InitializeForNewBrowser();
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
        NavigateBackToBrowsers();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        //if (ViewModel != null)
        //    ViewModel.Revert();
        NavigateBackToBrowsers();
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

    private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        if (args.Index == 0)
        {
            NavigateBackToBrowsers();
        }
    }

    private void NavigateBackToBrowsers()
    {
        if (Frame == null)
        {
            return;
        }

        if (Frame.CanGoBack)
        {
            Frame.GoBack();
            return;
        }

        Frame.Navigate(typeof(Hurl.App.Views.BrowsersPage));
    }
}
