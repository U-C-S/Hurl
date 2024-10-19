using Hurl.Settings.Views;
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

    private void NavView_Navigate(Type navPageType, NavigationTransitionInfo transitionInfo)
    {
        {
            NavView.Header = "Debug: Navigate"; // debug only

            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            //if (navPageType is not null) // debug only
            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
            {
                NavView.Header = "Debug: IF"; // debug only
                ContentFrame.Navigate(navPageType, null, transitionInfo);
            }
            else
            {
                NavView.Header = "Debug: ELSE"; // debug only
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

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        Type selectedPage = ((NavigationViewItem)args.SelectedItem).Tag switch
        {
            "General" => typeof(GeneralPage),
            "Rulesets" => typeof(RulesetsPage),
            "Browsers" => typeof(BrowsersPage),
            "About" => typeof(AboutPage),
            _ => typeof(GeneralPage),
        };

        ContentFrame.Navigate(selectedPage);
    }
}
