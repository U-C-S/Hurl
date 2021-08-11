using Hurl.Browser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Hurl.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowserSelectPage : Page
    {
        public BrowserSelectPage()
        {
            InitializeComponent();

            string[] x = Environment.GetCommandLineArgs();
            if (x.Length > 0)
            {
                arg = argss.Text = x[0];
            }
        }

        private string arg = null;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BList x = BList.InitalGetList();
            foreach (BrowserObject i in x)
            {
                if (i.Name != null)
                {
                    Button button = new Button()
                    {
                        Padding = new Thickness(5),
                        Margin = new Thickness(20, 5, 20, 0),
                        Content = i.Name,
                        Tag = i.ExePath,
                        //Style = (Style) FindResource("MDIXButton")
                    };

                    button.Click += BroClickAsync;
                    stacky.Children.Add(button);
                }

            }
        }

        private async void BroClickAsync(object sender, RoutedEventArgs e)
        {

            string path = (sender as Button).Tag.ToString();
            argss.Text = path;
            _ = Process.Start(path);
            //_ = ProcessLauncher.RunToCompletionAsync(path, arg);
            //Uri uri = new Uri(path);
            //_ = await Launcher.LaunchUriAsync(uri);
            //await ProcessLauncher.RunToCompletionAsync("ChromeGroup","mow");

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                // store command line parameters in local settings
                // so the Lancher can retrieve them and pass them on
                ApplicationData.Current.LocalSettings.Values["browser"] = path;
                //ApplicationData.Current.LocalSettings.Values["args"] = argss;


                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();

                // Thanks to:
                // https://stefanwick.com/2018/04/06/uwp-with-desktop-extension-part-1/
            }

        }
    }
}
