using System;
using System.Runtime.InteropServices;

namespace Hurl.Selector.Helpers;

internal static partial class CursorPosition
{
    private const uint MonitorDefaultToNearest = 0x00000002;

    public static ScreenPoint LimitCursorWithin(int width, int height)
    {
        if (!GetCursorPos(out var cursor))
        {
            return new ScreenPoint(0, 0);
        }

        var monitor = MonitorFromPoint(cursor, MonitorDefaultToNearest);
        MonitorInfo monitorInfo = new()
        {
            CbSize = Marshal.SizeOf<MonitorInfo>()
        };

        if (monitor == IntPtr.Zero || !GetMonitorInfo(monitor, ref monitorInfo))
        {
            return new ScreenPoint(cursor.X - width / 2, cursor.Y - height / 2);
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

        return new ScreenPoint(x, y);
    }

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetCursorPos(out ScreenPoint lpPoint);

    [LibraryImport("user32.dll")]
    private static partial IntPtr MonitorFromPoint(ScreenPoint pt, uint dwFlags);

    [LibraryImport("user32.dll", EntryPoint = "GetMonitorInfoW")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

    [StructLayout(LayoutKind.Sequential)]
    public struct ScreenPoint(int x, int y)
    {
        public int X = x;
        public int Y = y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MonitorInfo
    {
        public int CbSize;
        public Rect RcMonitor;
        public Rect RcWork;
        public uint DwFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
