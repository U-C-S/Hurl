using Hurl.Settings.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hurl.Settings
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            // TO-DO: winui3gallery://item/SystemBackdrops
            SystemBackdrop = new DesktopAcrylicBackdrop();
            AppWindow.SetIcon("internet.ico");
            AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(720,540));
            ExtendsContentIntoTitleBar = true;
        }

        private double NavViewCompactModeThresholdWidth => NavView.CompactModeThresholdWidth;

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            //not used!
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Type selectedPage = ((NavigationViewItem)args.SelectedItem).Tag switch
            {
                "General" => typeof(GeneralPage),
                "Browsers" => typeof(BrowsersPage),
                "Rulesets" => typeof(RulesetsPage),
                _ => typeof(GeneralPage),
            };

            NavFrame.Navigate(selectedPage);
            NavView.Header = sender.Content as string;
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            NavFrame.Loaded += NavView_Loaded;
            NavView.SelectedItem = NavView.MenuItems[0];
        }

        private void NavFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
