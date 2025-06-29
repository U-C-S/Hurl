using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Hurl.BrowserSelector.Helpers
{
    public static class CursorPosition
    {
        public static Point LimitCursorWithin(int width, int height)
        {
            var cursor = Cursor.Position;
            var screen = Screen.GetWorkingArea(cursor);
            var finalPoint = new Point(cursor.X - width / 2, cursor.Y - height / 2);

            if (cursor.X + width / 2 > screen.Right)
            {
                finalPoint.X = screen.Right - width;
            }
            if (cursor.X - width / 2 < screen.Left)
            {
                finalPoint.X = screen.Left;
            }
            if (cursor.Y + height / 2 > screen.Bottom)
            {
                finalPoint.Y = screen.Bottom - height;
            }
            if (cursor.Y - height / 2 < screen.Top)
            {
                finalPoint.Y = screen.Top;
            }

            Debug.WriteLine($"{finalPoint.X}�{finalPoint.Y} with screen resolution: {screen.Width}�{screen.Height}");

            return finalPoint;
        }
    }
}
