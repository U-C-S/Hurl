using Microsoft.UI.Xaml;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hurl.Timely
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.ExtendsContentIntoTitleBar = true;

            //window.AppWindow.IsShownInSwitchers = false;
            this.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(600, 320));

            this.InitializeComponent();
        }
    }
}
