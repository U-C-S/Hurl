using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

namespace Hurl.Selector.Services.Interfaces;

public interface IIconLoader
{
    Task<BitmapImage?> LoadIconFromExe(string exePath);
    Task<BitmapImage> LoadIconFromExe(string exePath, int iconIndex);
    Task<BitmapImage> LoadIconFromIco(string icoPath);
    Task<BitmapImage> LoadIconFromImage(string imagePath);
    Task<BitmapImage> LoadIconFromURL(string url);
}
