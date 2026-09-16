using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.IO;
using Windows.Graphics;

namespace Hurl.App.Windows;

public sealed partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        Title = "Hurl Settings";
        AppWindow.ResizeClient(new SizeInt32(1320, 900));
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets", "internet.ico"));
        SystemBackdrop = new MicaBackdrop();
    }

    private void OnNavItemClicked(object sender, ItemClickEventArgs e)
    {
        var item = e.ClickedItem as ListViewItem
            ?? (e.ClickedItem as FrameworkElement)?.FindParent<ListViewItem>();
        if (item?.Tag is string page)
        {
            NavigateToPage(page);
        }
    }

    public void NavigateToPage(string page)
    {
        (Type pageType, int index) = page.ToLowerInvariant() switch
        {
            "about" => (typeof(Views.AboutPage), -1),
            "rulesets" => (typeof(Views.RulesetPage), 1),
            "quickview" => (typeof(Views.QuickViewPage), 2),
            "settings" => (typeof(Views.SettingsPage), 3),
            _ => (typeof(Views.BrowsersPage), 0)
        };

        if (NavigationFrame.CurrentSourcePageType != pageType)
        {
            NavigationFrame.Navigate(pageType);
        }
        NavMenuHeaderList.SelectedIndex = index;
        NavMenuFooterList.SelectedIndex = index == -1 ? 0 : -1;
    }
}
