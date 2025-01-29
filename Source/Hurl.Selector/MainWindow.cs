using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Hurl.Selector;
internal class MainWindow
{
    readonly Window window;
    public MainWindow()
    {
        window = new Window()
        {
            SystemBackdrop = new DesktopAcrylicBackdrop(),
            ExtendsContentIntoTitleBar = true,
            Title = "Hurl Selector Preview",
        };
        //window.AppWindow.IsShownInSwitchers = false;
        window.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(600, 320));

        var wndMng = WinUIEx.WindowManager.Get(window);
        wndMng.IsMaximizable = false;
        wndMng.IsMinimizable = false;
        wndMng.MinWidth = 500;
    }

    public Window GetWindow => window;
}
