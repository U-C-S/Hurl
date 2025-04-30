using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using WinUIEx;

namespace Hurl.Selector;

internal class MainWindow
{
    Window? window;
    WindowManager? windowManager;

    public void CreateWindow()
    {
        if (window == null)
        {
            window = new Window()
            {
                SystemBackdrop = new MicaBackdrop(),
                ExtendsContentIntoTitleBar = true,
                Title = "Hurl Selector Preview",
            };

            windowManager = WindowManager.Get(window);
            windowManager.IsMaximizable = false;
            windowManager.IsMinimizable = false;
#if DEBUG == false
            windowManager.AppWindow.IsShownInSwitchers = false;
#endif
            windowManager.MinWidth = 500;
            windowManager.MinHeight = 250;

            //window.AppWindow.IsShownInSwitchers = false;
            window.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(600, 320));
        }
    }

    public void Show() => window?.Activate();

    public void SetContent(UIElement content) => window.Content = content;
}
