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

        this.AppWindow.ResizeClient(new SizeInt32(720, 540));
        this.AppWindow.SetIcon("internet.ico");
        this.SystemBackdrop = new MicaBackdrop();

        NavigateToPage("browsers");
    }

    private void OnNavItemClicked(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is FrameworkElement f && f.FindParent<ListViewItem>() is { Tag: string tag })
        {
            NavigateToPage(tag);
        }
    }

    public void NavigateToPage(string page)
    {
        switch (page)
        {
            case "general":
                NavigationFrame.Navigate(typeof(Views.GeneralPage));
                break;
            case "browsers":
                NavigationFrame.Navigate(typeof(Views.BrowsersPage));
                //ViewModel.Navigate(ContentPageType.Apps);
                break;
            case "rulesets":
                NavigationFrame.Navigate(typeof(Views.RulesetsPage));
                break;
            case "about":
                NavigationFrame.Navigate(typeof(Views.AboutPage));
                break;
        }
    }
}
