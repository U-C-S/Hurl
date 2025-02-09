using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hurl.Library;

public static class IconExtractor
{
    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    internal static extern UInt32 PrivateExtractIcons(String lpszFile, int nIconIndex, int cxIcon, int cyIcon, IntPtr[] phicon, IntPtr[] piconid, UInt32 nIcons, UInt32 flags);

    public static Icon? FromFile(string filename)
    {
        try
        {
            IntPtr[] phicon = [IntPtr.Zero];
            IntPtr[] piconid = [IntPtr.Zero];

            PrivateExtractIcons(filename, 0, 128, 128, phicon, piconid, 1, 0);

            return phicon[0] != IntPtr.Zero ? Icon.FromHandle(phicon[0]) : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
}

// Thanks to https://stackoverflow.com/a/1127795
public static class IconUtilites
{
    // Avoids memory leak
    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern bool DeleteObject(IntPtr hObject);

    public static ImageSource? ToImageSource(this Icon icon)
    {
        if (icon is null)
        {
            return null;
        }
        try
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return !DeleteObject(hBitmap) ? throw new Win32Exception() : wpfBitmap;
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
