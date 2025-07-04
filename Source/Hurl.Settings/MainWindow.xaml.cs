using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;

namespace Hurl.Settings;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        Title = "Hurl Settings Preview";

        this.AppWindow.ResizeClient(new SizeInt32(1200, 840));
        this.AppWindow.SetIcon("internet.ico");
        this.SystemBackdrop = new MicaBackdrop();

        NavigateToPage("browsers");
    }

    private string _currentPage = string.Empty;

    private void OnNavItemClicked(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is FrameworkElement f && f.FindParent<ListViewItem>() is { Tag: string tag })
        {
            NavigateToPage(tag);
        }
    }

    public void NavigateToPage(string page)
    {
        if (_currentPage == page)
            return;
        else _currentPage = page;

        switch (page)
        {
            case "about":
                NavigationFrame.Navigate(typeof(Views.AboutPage));
                break;
            case "browsers":
                NavigationFrame.Navigate(typeof(Views.BrowsersPage));
                NavMenuHeaderList.SelectedIndex = 0;
                //ViewModel.Navigate(ContentPageType.Apps);
                break;
            case "rulesets":
                NavigationFrame.Navigate(typeof(Views.RulesetPage));
                break;
            case "settings":
                NavigationFrame.Navigate(typeof(Views.Settings));
                break;
        }

        if (page == "about")
        {
            NavMenuFooterList.SelectedIndex = 0;
            NavMenuHeaderList.SelectedIndex = -1;
        }
        else
        {
            NavMenuFooterList.SelectedIndex = -1;
        }
    }
}
