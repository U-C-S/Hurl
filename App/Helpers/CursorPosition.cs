using System.Runtime.InteropServices;
using System.Windows;

namespace Hurl.BrowserSelector.Helpers
{
    public static class CursorPosition
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        };
        public static Point Get()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public static Point LimitCursorWithin(int width, int height)
        {
            var Cursor = Get();
            var finalPoint = new Point(Cursor.X, Cursor.Y);
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;

            if (Cursor.X + width > screenWidth)
            {
                finalPoint.X = screenWidth - width;
            }
            if (Cursor.Y + height > screenHeight)
            {
                finalPoint.Y = screenHeight - height;
            }

            return finalPoint;
        }
    }
}
