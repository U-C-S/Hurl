using System.Runtime.InteropServices;

namespace Hurl.Selector.Helpers;

internal static partial class KeyboardState
{
    private const int KeyDownMask = 0x8000;
    private const int VirtualKeyMenu = 0x12;
    private const int VirtualKeyControl = 0x11;

    public static bool IsAltKeyDown()
    {
        return (GetAsyncKeyState(VirtualKeyMenu) & KeyDownMask) != 0;
    }

    public static bool IsCtrlKeyDown()
    {
        return (GetAsyncKeyState(VirtualKeyControl) & KeyDownMask) != 0;
    }

    [LibraryImport("user32.dll")]
    private static partial short GetAsyncKeyState(int virtualKey);
}
