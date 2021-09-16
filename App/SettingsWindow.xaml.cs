using Hurl.Constants;
using Hurl.Controls;
using Hurl.Services;
using Hurl.Views;
using System;
using System.Security.Principal;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Hurl
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private Installer InstallerService;

        public SettingsWindow()
        {
            InitializeComponent();
            LoadSystemBrowserList();

            InstallerService = new Installer();

            if (InstallerService.isDefault)
            {
                DefaultInfo.Text = "Hurl is currently the default handler for http/https links";
                DefaultSetButton.IsEnabled = false;
            }

            if (InstallerService.hasProtocol)
            {
                ProtocolInfo.Text = "Protocol is installed and Avaliable through hurl://";
                ProtocolSetButton.IsEnabled = false;
            }
        }

        //private void Uninstall_Button(object sender, RoutedEventArgs e) => InstallerService.Uninstall();

        private void SetAsDefualt(object sender, RoutedEventArgs e) => InstallerService.SetDefault();

        private void Protocol_Button(object sender, RoutedEventArgs e) => InstallerService.ProtocolRegister();

        //Browsers Tab
        public void LoadSystemBrowserList()
        {
            GetBrowsers x = GetBrowsers.InitalGetList();

            foreach (BrowserObject i in x)
            {
                if (i.Name != null)
                {
                    var image = Imaging.CreateBitmapSourceFromHBitmap(i.GetIcon.ToBitmap().GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    var comp = new BrowserStatusComponent
                    {
                        BrowserName = i.Name,
                        BrowserPath = i.ExePath,
                        Img = image,
                        EditEnabled = true,
                        BackColor = "#FFFFDAAD",
                        Margin = new Thickness(0, 4, 0, 0),
                    };
                    //comp.DeleteItem += DeleteBrowser;
                    _ = StackSystemBrowsers.Children.Add(comp);
                }

                if(i.GetIcon == null)
                {
                    System.Windows.Forms.MessageBox.Show("lol");
                }

            }
        }

        //Add browsers
        private void AddBrowser(object sender, RoutedEventArgs e)
        {
            BrowserForm f = new BrowserForm();
            if (f.ShowDialog() == true)
            {
                var comp = new BrowserStatusComponent
                {
                    BrowserName = f.BrowserName,
                    BrowserPath = f.BrowserPath,
                    EditEnabled = true,
                    BackColor = "#FFFFDAAD",
                    Margin = new Thickness(0, 4, 0, 0),
                };
                StackUserBrowsers.Children.Add(comp);

            }
        }

        private void RefreshBrowserList(object sender, RoutedEventArgs e)
        {
            StackSystemBrowsers.Children.Clear();
            LoadSystemBrowserList();
        }
    }
}
