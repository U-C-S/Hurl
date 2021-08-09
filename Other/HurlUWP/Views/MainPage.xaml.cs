using HurlUWP.Browser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MUXC = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HurlUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            //PreviewKeyDown += new KeyEventHandler(Window_Esc);

            var x = Environment.GetCommandLineArgs();
            if(x.Length >= 2)
            {
                arg = argss.Text = x[1];
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

                    button.Click += BroClick;
                    stacky.Children.Add(button);
                }

            }
        }

        private async void BroClick(object sender, RoutedEventArgs e)
        {
            if (arg != null)
            {
                string path = (sender as Button).Tag.ToString();
                argss.Text = path;
                //_ = ProcessLauncher.RunToCompletionAsync(path, arg);
                Uri uri = new Uri(path);
                _ = await Launcher.LaunchUriAsync(uri);
            }
        }

    }
}
