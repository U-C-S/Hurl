using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.UI.ApplicationSettings;

namespace Hurl.Settings
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            //SetTitleBar(AppTitleBar);
            Title = "Hurl Settings Preview";

            this.AppWindow.ResizeClient(new SizeInt32(1200, 840));
            this.SystemBackdrop = new MicaBackdrop();
        }

        private void OnNavItemClicked(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is FrameworkElement f && f.FindParent<ListViewItem>() is { Tag: string tag })
            {

                switch (tag)
                {
                    case "about":
                        NavigationFrame.Navigate(typeof(Views.AboutPage));
                        break;
                    case "browsers":
                        NavigationFrame.Navigate(typeof(Views.BrowsersPage));
                        //ViewModel.Navigate(ContentPageType.Apps);
                        break;
                    case "rulesets":
                        NavigationFrame.Navigate(typeof(Views.RulesetPage));
                        break;
                    case "settings":
                        NavigationFrame.Navigate(typeof(Views.Settings));
                        break;
                }

                if (tag == "about")
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
