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
            SystemBackdrop = new MicaBackdrop(),
            ExtendsContentIntoTitleBar = true,
            Title = "Hurl Selector Preview"
        };
        window.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(500, 250));
    }

    public Window GetWindow => window;
}
