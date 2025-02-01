using Hurl.Selector.Services.Interfaces;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Hurl.Selector.Services;

public class IconLoaderService : IIconLoader
{
    public async Task<BitmapImage?> LoadIconFromExe(string exePath)
    {
        try
        {
            var file = await StorageFile.GetFileFromPathAsync(exePath);
            var thumb = await file.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem);
            if (thumb != null)
            {
                var bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(thumb);
                return bitmap;
            }
        }
        catch { }

        return null;
    }

    public Task<BitmapImage> LoadIconFromExe(string exePath, int iconIndex)
    {
        throw new NotImplementedException();
    }

    public Task<BitmapImage> LoadIconFromIco(string icoPath)
    {
        throw new NotImplementedException();
    }

    public Task<BitmapImage> LoadIconFromImage(string imagePath)
    {
        throw new NotImplementedException();
    }

    public Task<BitmapImage> LoadIconFromURL(string url)
    {
        throw new NotImplementedException();
    }
}
