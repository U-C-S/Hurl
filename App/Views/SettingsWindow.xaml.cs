using Hurl.Browser;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Hurl.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            LoadSystemBrowserList();
            InstallPathTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Hurl";
        }

        public void LoadSystemBrowserList()
        {
            BList x = BList.InitalGetList();

            foreach (BrowserObject i in x)
            {
                if (i.Name != null)
                {
                    TextBlock text = new TextBlock()
                    {
                        Padding = new Thickness(2),
                        Text = $"{i.Name} -- {i.ExePath}"
                    };
                    _ = StackSystemBrowsers.Children.Add(text);
                }

            }
        }
    }
}
