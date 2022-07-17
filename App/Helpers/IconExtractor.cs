using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hurl.BrowserSelector.Helpers
{
    public static class IconExtractor
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern UInt32 PrivateExtractIcons(String lpszFile, int nIconIndex, int cxIcon, int cyIcon, IntPtr[] phicon, IntPtr[] piconid, UInt32 nIcons, UInt32 flags);

        public static Icon FromFile(string filename)
        {
            try
            {
                IntPtr[] phicon = new IntPtr[] { IntPtr.Zero };
                IntPtr[] piconid = new IntPtr[] { IntPtr.Zero };

                PrivateExtractIcons(filename, 0, 128, 128, phicon, piconid, 1, 0);

                if (phicon[0] != IntPtr.Zero)
                    return Icon.FromHandle(phicon[0]);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }

    // Thanks to https://stackoverflow.com/a/1127795
    internal static class IconUtilites
    {
        // Avoids memory leak
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ToImageSource(this Icon icon)
        {
            try
            {
                Bitmap bitmap = icon.ToBitmap();
                IntPtr hBitmap = bitmap.GetHbitmap();

                ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                if (!DeleteObject(hBitmap))
                {
                    throw new Win32Exception();
                }
                return wpfBitmap;
            }
            catch (Exception ex)
            {
                //TODO: On error, use a default icon
                //ImageSource wpfBitmap = Imaging;
                Debug.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
