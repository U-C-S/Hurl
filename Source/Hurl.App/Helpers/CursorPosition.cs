using System;
using System.Runtime.InteropServices;

namespace Hurl.App.Helpers;

internal static class CursorPosition
{
    private const uint MonitorDefaultToNearest = 0x00000002;

    public static NativeMethods.ScreenPoint LimitCursorWithin(int width, int height)
    {
        if (!NativeMethods.GetCursorPos(out var cursor))
        {
            return new NativeMethods.ScreenPoint(0, 0);
        }

        var monitor = NativeMethods.MonitorFromPoint(cursor, MonitorDefaultToNearest);
        NativeMethods.MonitorInfo monitorInfo = new()
        {
            CbSize = Marshal.SizeOf<NativeMethods.MonitorInfo>()
        };

        if (monitor == IntPtr.Zero || !NativeMethods.GetMonitorInfo(monitor, ref monitorInfo))
        {
            return new NativeMethods.ScreenPoint(cursor.X - width / 2, cursor.Y - height / 2);
        }

        var workArea = monitorInfo.RcWork;
        var x = cursor.X - width / 2;
        var y = cursor.Y - height / 2;

        if (cursor.X + width / 2 > workArea.Right)
        {
            x = workArea.Right - width;
        }
        if (cursor.X - width / 2 < workArea.Left)
        {
            x = workArea.Left;
        }
        if (cursor.Y + height / 2 > workArea.Bottom)
        {
            y = workArea.Bottom - height;
        }
        if (cursor.Y - height / 2 < workArea.Top)
        {
            y = workArea.Top;
        }

        return new NativeMethods.ScreenPoint(x, y);
    }
}
