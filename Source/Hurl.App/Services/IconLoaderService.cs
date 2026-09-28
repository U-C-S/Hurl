using Hurl.App.Helpers;
using Hurl.App.Services.Interfaces;
using Hurl.Library;
using Hurl.Library.Models;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.App.Services;

public class IconLoaderService : IIconLoader
{
    /// <summary>
    /// The selector takes up 80x80 pixels. so size 256 can cover display scaling till 300%.
    /// </summary>
    private const int IconSize = 256;
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(10) };
    private readonly string cacheDirectory;

    public IconLoaderService(string? cacheDirectory = null)
    {
        this.cacheDirectory = cacheDirectory ?? Path.Combine(Constants.APP_SETTINGS_DIR, "cache", "icons");
    }

    #region Primary Methods
    public async Task<BitmapImage?> LoadIconAsync(Browser browser)
    {
        var IconConfig = browser.Icon;
        if (!string.IsNullOrWhiteSpace(IconConfig?.Path))
        {
            var customIcon = IconConfig.Source switch
            {
                BrowserIconSource.LocalImage => await LoadIconFromImage(IconConfig.Path),
                BrowserIconSource.Url => await LoadIconFromURL(IconConfig.Path),
                _ => null
            };
            if (customIcon is not null) return customIcon;
        }

        if (string.IsNullOrWhiteSpace(browser.ExePath)) return null;

        int index = IconConfig?.Source == BrowserIconSource.Executable ? Math.Max(0, IconConfig.Index) : 0;
        var icon = await LoadIconFromExe(browser.ExePath, index);
        return icon ?? (index != 0 ? await LoadIconFromExe(browser.ExePath) : null);
    }

    public Task<BitmapImage?> LoadIconFromExe(string exePath) => LoadIconFromExe(exePath, 0);

    public Task<BitmapImage?> LoadIconFromExe(string exePath, int iconIndex) =>
        LoadLocalIconAsync(exePath, iconIndex);

    public Task<int> GetExeIconCountAsync(string exePath) => Task.Run(() =>
    {
        try
        {
            if (string.IsNullOrWhiteSpace(exePath)) return 0;
            string path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(exePath.Trim().Trim('"')));
            if (!File.Exists(path)) return 0;

            uint count = NativeMethods.ExtractIconEx(path, -1, IntPtr.Zero, IntPtr.Zero, 0);
            return count <= int.MaxValue ? (int)count : 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Could not enumerate executable icons: {ex.Message}");
            return 0;
        }
    });

    public Task<BitmapImage?> LoadIconFromIco(string icoPath) => LoadIconFromImage(icoPath);

    public Task<BitmapImage?> LoadIconFromImage(string imagePath) => LoadLocalIconAsync(imagePath);

    public async Task<BitmapImage?> LoadIconFromURL(string url)
    {
        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return null;
            }

            // Remote images are reused until the cache is cleared or the URL changes.
            return await LoadCachedIconAsync($"url|{uri.AbsoluteUri}", ".img",
                () => HttpClient.GetByteArrayAsync(uri));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Could not load icon from Url: {ex.Message}");
            return null;
        }
    }
    #endregion

    #region Helper Methods
    private async Task<BitmapImage?> LoadLocalIconAsync(string path, int? iconIndex = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(path.Trim().Trim('"')));
            var file = new FileInfo(path);
            if (!file.Exists)
            {
                return null;
            }

            string key = $"icon|{path.ToUpperInvariant()}|{file.Length}|{file.LastWriteTimeUtc.Ticks}|{iconIndex}|{IconSize}";
            string extension = iconIndex.HasValue ? ".png" : Path.GetExtension(path);
            return await LoadCachedIconAsync(key, extension, () => iconIndex.HasValue
                ? Task.Run(() => ExtractIcon(path, iconIndex.Value))
                : File.ReadAllBytesAsync(path));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Could not load local icon: {ex.Message}");
            return null;
        }
    }

    private static byte[] ExtractIcon(string path, int iconIndex)
    {
        using var icon = Icon.ExtractIcon(path, iconIndex, IconSize)
            ?? throw new IOException($"Icon {iconIndex} was not found in '{path}'.");
        using var bitmap = icon.ToBitmap();
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        return stream.ToArray();
    }
    #endregion

    #region Cache Helper Methods
    private async Task<BitmapImage> LoadCachedIconAsync(string key, string extension, Func<Task<byte[]>> loadSource)
    {
        string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(key)));
        string cachePath = Path.Combine(cacheDirectory, hash + extension.ToLowerInvariant());
        if (File.Exists(cachePath))
        {
            try
            {
                return await DecodeAsync(await File.ReadAllBytesAsync(cachePath));
            }
            catch (Exception ex)
            {
                // A corrupt or inaccessible cache entry must not prevent loading its source.
                Debug.WriteLine($"Could not read cached icon: {ex.Message}");
            }
        }

        byte[] bytes = await loadSource();
        var bitmap = await DecodeAsync(bytes);
        // Only publish images that have successfully decoded.
        await TryCacheAsync(cachePath, bytes);
        return bitmap;
    }

    private static async Task<BitmapImage> DecodeAsync(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes, writable: false);
        using var randomAccessStream = stream.AsRandomAccessStream();
        var bitmap = new BitmapImage();
        await bitmap.SetSourceAsync(randomAccessStream);
        return bitmap;
    }

    private async Task TryCacheAsync(string cachePath, byte[] bytes)
    {
        string temporaryPath = cachePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            Directory.CreateDirectory(cacheDirectory);
            await File.WriteAllBytesAsync(temporaryPath, bytes);
            // A unique temporary file and atomic replacement keep concurrent readers safe.
            File.Move(temporaryPath, cachePath, overwrite: true);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Could not cache icon: {ex.Message}");
        }
        finally
        {
            try
            {
                File.Delete(temporaryPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could not remove temporary icon: {ex.Message}");
            }
        }
    }
    #endregion
}
