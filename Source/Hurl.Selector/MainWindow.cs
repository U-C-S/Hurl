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
        window.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(500, 250));

        var wndMng = WinUIEx.WindowManager.Get(window);
        wndMng.IsMaximizable = false;
        wndMng.IsMinimizable = false;
    }

    public Window GetWindow => window;
}
