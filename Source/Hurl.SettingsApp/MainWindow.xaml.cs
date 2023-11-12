// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.WinUI;
using Hurl.SettingsApp.ViewModels;
using Hurl.SettingsApp.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hurl.SettingsApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            Title = "Hurl Settings";

            this.AppWindow.ResizeClient(new SizeInt32(1200, 640));
            this.SystemBackdrop = new MicaBackdrop();
        }

        //public static MainWindowViewModel ViewModel => new();

        private void OnNavItemClicked(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is FrameworkElement f && f.FindParent<ListViewItem>() is { Tag: string tag })
            {

                switch (tag)
                {
                    case "home":
                        NavigationFrame.Navigate(typeof(HomePage));
                        break;
                    case "apps":
                        NavigationFrame.Navigate(typeof(AppsPage));
                        //ViewModel.Navigate(ContentPageType.Apps);
                        break;
                    case "rules":
                        NavigationFrame.Navigate(typeof(AddRulePage)); 
                        break;
                    case "settings":
                        NavigationFrame.Navigate(typeof(SettingsPage)); ;
                        break;
                }

                if (tag == "settings")
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
    }
}
