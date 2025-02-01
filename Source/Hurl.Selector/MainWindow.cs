using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using WinUIEx;

namespace Hurl.Selector;
internal class MainWindow
{
    readonly Window window;
    readonly WindowManager windowManager;
    public MainWindow()
    {
        window = new Window()
        {
            SystemBackdrop = new MicaBackdrop(),
            ExtendsContentIntoTitleBar = true,
            Title = "Hurl Selector Preview",
        };
        //window.AppWindow.IsShownInSwitchers = false;
        window.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(600, 320));

        windowManager = WinUIEx.WindowManager.Get(window);
        windowManager.IsMaximizable = false;
        windowManager.IsMinimizable = false;
        windowManager.MinWidth = 500;
        windowManager.MinHeight = 200;
    }

    public void Show() => window.Activate();

    public void SetContent(UIElement content) => window.Content = content;
}
