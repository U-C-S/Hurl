using Hurl.Library.Models;
using Hurl.Selector.Services.Interfaces;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Hurl.Selector.Services;

public class IconLoaderService : IIconLoader
{
    public async Task<BitmapImage?> LoadIconAsync(Browser browser)
    {
        if (!string.IsNullOrWhiteSpace(browser.CustomIconPath))
        {
            string customIconPath = browser.CustomIconPath.Trim('"');
            return string.Equals(Path.GetExtension(customIconPath), ".ico", StringComparison.OrdinalIgnoreCase)
                ? await LoadIconFromIco(customIconPath)
                : await LoadIconFromImage(customIconPath);
        }

        if (!string.IsNullOrWhiteSpace(browser.ExePath))
        {
            return await LoadIconFromExe(browser.ExePath.Trim('"'));
        }

        return null;
    }

    public async Task<BitmapImage?> LoadIconFromExe(string exePath)
    {
        try
        {
            var file = await StorageFile.GetFileFromPathAsync(exePath);
            var thumb = await file.GetThumbnailAsync(ThumbnailMode.SingleItem);
            if (thumb != null)
            {
                var bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(thumb);
                return bitmap;
            }
        }
        catch
        {
        }

        return null;
    }

    public Task<BitmapImage?> LoadIconFromExe(string exePath, int iconIndex)
    {
        // TODO: Implement icon index handling. For now, load the default icon.
        return LoadIconFromExe(exePath);
    }

    public Task<BitmapImage?> LoadIconFromIco(string icoPath)
    {
        return LoadBitmapFromFileAsync(icoPath);
    }

    public Task<BitmapImage?> LoadIconFromImage(string imagePath)
    {
        return LoadBitmapFromFileAsync(imagePath);
    }

    public Task<BitmapImage?> LoadIconFromURL(string url)
    {
        try
        {
            return Task.FromResult<BitmapImage?>(new BitmapImage(new Uri(url)));
        }
        catch
        {
            return Task.FromResult<BitmapImage?>(null);
        }
    }

    private static async Task<BitmapImage?> LoadBitmapFromFileAsync(string path)
    {
        try
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            using var stream = await file.OpenReadAsync();
            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(stream);
            return bitmap;
        }
        catch
        {
            return null;
        }
    }
}
