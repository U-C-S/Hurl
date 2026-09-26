namespace Hurl.App.Helpers;

internal static class KeyboardState
{
    private const int KeyDownMask = 0x8000;
    private const int VirtualKeyMenu = 0x12;
    private const int VirtualKeyControl = 0x11;

    public static bool IsAltKeyDown()
    {
        return (NativeMethods.GetAsyncKeyState(VirtualKeyMenu) & KeyDownMask) != 0;
    }

    public static bool IsCtrlKeyDown()
    {
        return (NativeMethods.GetAsyncKeyState(VirtualKeyControl) & KeyDownMask) != 0;
    }
}
