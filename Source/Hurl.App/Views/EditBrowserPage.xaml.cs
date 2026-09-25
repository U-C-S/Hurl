using Hurl.Library.Models;
using Hurl.App.ViewModels;
using Hurl.App.Services.Interfaces;
using Hurl.App.Views.Dialogs;
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
    private bool isChoosingIcon;

    public EditBrowserPage()
    {
        InitializeComponent();
    }

    public EditBrowserPage(Browser browser)
    {
        InitializeComponent();
        InitializeForBrowser(browser);
    }

    private void InitializeForBrowser(Browser browser, bool isNewBrowser = false)
    {
        var settingsService = App.Services!.GetRequiredService<ISettingsService>();
        var iconLoader = App.Services!.GetRequiredService<IIconLoader>();

        BreadcrumbItems.Add(isNewBrowser ? "New Browser" : browser.Name);
        ViewModel = new EditBrowserPageViewModel(browser, settingsService, iconLoader, isNewBrowser);
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
            InitializeForBrowser(new Browser(), isNewBrowser: true);
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

    private async void ChooseIcon_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel is null || isChoosingIcon) return;
        isChoosingIcon = true;
        try
        {
            var dialog = new BrowserIconDialog(App.Services!.GetRequiredService<IIconLoader>(), ViewModel.ExePath, ViewModel.Icon)
            {
                XamlRoot = XamlRoot,
                RequestedTheme = ActualTheme
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary && dialog.ViewModel.Selection is { } choice)
            {
                ViewModel.ApplyIcon(choice.Icon, choice.Image);
            }
        }
        finally
        {
            isChoosingIcon = false;
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
