using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
//using System.Windows;
using Windows.Graphics;
namespace Hurl.Settings;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;

        AppWindow.ResizeClient(new SizeInt32(720, 540));
        AppWindow.SetIcon("internet.ico");
        SystemBackdrop = new MicaBackdrop();
    }

    private double NavViewCompactModeThresholdWidth { get { return NavView.CompactModeThresholdWidth; } }

    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer != null)
        {
            Type navPageType = Type.GetType(args.InvokedItemContainer.Tag.ToString());
            NavView_Navigate(navPageType, args.RecommendedNavigationTransitionInfo);
        }
    }

    private void NavView_Navigate(Type navPageType, NavigationTransitionInfo transitionInfo)
    {
        {
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type preNavPageType = ContentFrame.CurrentSourcePageType;

            NavView.Header = "Test Navigate"; // debug only

            // Only navigate if the selected page isn't currently loaded.
            //if (navPageType is not null)
            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
            {
                NavView.Header = "Test IF"; // debug only
                ContentFrame.Navigate(navPageType, null, transitionInfo);
            }
            else
            {
                NavView.Header = "Test ELSE"; // debug only
            }
        }
    }

    private void NavView_Loaded(object sender, RoutedEventArgs e)
    {
        // Add handler for ContentFrame navigation.
        ContentFrame.Navigated += On_Navigated;

        // NavView doesn't load any page by default, so load home page.
        NavView.SelectedItem = NavView.MenuItems[0];

        // If navigation occurs on SelectionChanged, this isn't needed.
        // Because we use ItemInvoked to navigate, we need to call Navigate
        // here to load the home page.
        //NavView_Navigate(typeof(BrowsersPage), new EntranceNavigationTransitionInfo());
    }

    private void On_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void NavView_ItemSelected(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer != null)
        {
            NavView.Header = "Test Selected"; // debug only
            // --- HERE IT BREAKS!! ---
            Type navPageType = Type.GetType(args.SelectedItemContainer.Tag.ToString());
            //NavView.Header = navPageType.ToString(); // debug only
            NavView_Navigate(navPageType, args.RecommendedNavigationTransitionInfo);
        }
    }
}
