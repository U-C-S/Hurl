using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.App.Services.Interfaces;
using Hurl.Library.Models;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hurl.App.ViewModels;

public sealed record BrowserIconChoice(BrowserIcon Icon, BitmapImage Image, string Name, string Details, string Source);

public partial class BrowserIconDialogViewModel : ObservableObject
{
    private readonly IIconLoader iconLoader;
    private readonly string exePath;
    private readonly int originalIconIndex;
    private readonly string? originalLocalPath;
    private BrowserIconChoice? localChoice;
    private BrowserIconChoice? urlChoice;
    private bool executableIconsLoaded;
    private bool isClosed;

    public ObservableCollection<BrowserIconChoice> ExeIcons { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPreview))]
    [NotifyPropertyChangedFor(nameof(ShowEmptyState))]
    public partial BrowserIconSource SelectedSource { get; set; } = BrowserIconSource.Executable;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    [NotifyPropertyChangedFor(nameof(HasPreview))]
    [NotifyPropertyChangedFor(nameof(ShowEmptyState))]
    public partial BrowserIconChoice? Selection { get; set; }

    [ObservableProperty]
    public partial BrowserIconChoice? SelectedExeIcon { get; set; }

    [ObservableProperty]
    public partial string UrlText { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    [NotifyPropertyChangedFor(nameof(CanInteract))]
    [NotifyPropertyChangedFor(nameof(ShowEmptyState))]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public bool CanSave => !IsBusy && Selection is not null;
    public bool CanInteract => !IsBusy;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasPreview => SelectedSource != BrowserIconSource.Executable && Selection is not null;
    public bool ShowEmptyState => SelectedSource != BrowserIconSource.Executable && Selection is null && !IsBusy;

    public BrowserIconDialogViewModel(IIconLoader iconLoader, string exePath, BrowserIcon? icon)
    {
        this.iconLoader = iconLoader;
        this.exePath = exePath;
        originalIconIndex = icon?.Index ?? 0;
        if (icon?.Source == BrowserIconSource.Url)
        {
            UrlText = icon.Path ?? string.Empty;
            SelectedSource = BrowserIconSource.Url;
        }
        else if (icon?.Source == BrowserIconSource.LocalImage)
        {
            originalLocalPath = icon.Path;
            SelectedSource = BrowserIconSource.LocalImage;
        }
    }

    partial void OnSelectedSourceChanged(BrowserIconSource value)
    {
        ErrorMessage = string.Empty;
        Selection = value switch
        {
            BrowserIconSource.Executable => SelectedExeIcon,
            BrowserIconSource.LocalImage => localChoice,
            BrowserIconSource.Url => urlChoice,
            _ => null
        };
    }

    partial void OnSelectedExeIconChanged(BrowserIconChoice? value)
    {
        if (SelectedSource == BrowserIconSource.Executable) Selection = value;
    }

    partial void OnUrlTextChanged(string value)
    {
        urlChoice = null;
        if (SelectedSource == BrowserIconSource.Url)
        {
            Selection = null;
            ErrorMessage = string.Empty;
        }
    }

    public async Task SelectSourceAsync(BrowserIconSource source)
    {
        if (IsBusy || isClosed) return;
        SelectedSource = source;
        switch (source)
        {
            case BrowserIconSource.Executable:
                if (!executableIconsLoaded)
                    await LoadExecutableIconsAsync();
                else if (ExeIcons.Count == 0)
                    ErrorMessage = "No icons found. Check the executable path, or choose a local image or URL.";
                break;
            case BrowserIconSource.LocalImage when localChoice is null && originalLocalPath is not null:
                await LoadLocalImageAsync(originalLocalPath);
                break;
            case BrowserIconSource.Url when urlChoice is null && !string.IsNullOrWhiteSpace(UrlText):
                await LoadUrlAsync();
                break;
        }
    }

    private async Task LoadExecutableIconsAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            int count = await iconLoader.GetExeIconCountAsync(exePath);
            for (int index = 0; index < count && !isClosed; index++)
            {
                var image = await iconLoader.LoadIconFromExe(exePath, index);
                if (isClosed) return;
                if (image is null) continue;
                var choice = new BrowserIconChoice(new BrowserIcon { Source = BrowserIconSource.Executable, Index = index },
                    image, $"Icon {index}", Dimensions(image), exePath);
                ExeIcons.Add(choice);
                if (index == originalIconIndex) SelectedExeIcon = choice;
            }

            if (isClosed) return;
            executableIconsLoaded = true;
            SelectedExeIcon ??= ExeIcons.FirstOrDefault();
            if (ExeIcons.Count == 0)
                ErrorMessage = "No icons found. Check the executable path, or choose a local image or URL.";
        }
        finally
        {
            if (!isClosed) IsBusy = false;
        }
    }

    public Task LoadLocalImageAsync(string path) => LoadImageAsync(path, fromUrl: false);

    public async Task LoadUrlAsync()
    {
        if (IsBusy || isClosed) return;
        string url = UrlText.Trim();
        if (!IsWebUrl(url))
        {
            Selection = null;
            urlChoice = null;
            ErrorMessage = "Enter a direct image URL starting with https:// or http://.";
            return;
        }
        await LoadImageAsync(url, fromUrl: true);
    }

    private async Task LoadImageAsync(string source, bool fromUrl)
    {
        if (IsBusy || isClosed) return;
        IsBusy = true;
        ErrorMessage = string.Empty;
        Selection = null;
        if (fromUrl) urlChoice = null;
        else localChoice = null;
        try
        {
            var image = fromUrl
                ? await iconLoader.LoadIconFromURL(source)
                : await iconLoader.LoadIconFromImage(source);
            // Ignore work completed after Cancel, or a URL edited while the request was in flight.
            if (isClosed || (fromUrl && UrlText.Trim() != source)) return;
            if (image is null)
            {
                ErrorMessage = fromUrl
                    ? "Couldn't load this image. Check the URL and your connection, then try again."
                    : "Couldn't open this image. Choose a supported image file that is still available.";
                return;
            }

            string name = fromUrl ? new Uri(source).Host : Path.GetFileName(source);
            var choice = new BrowserIconChoice(new BrowserIcon
            {
                Source = fromUrl ? BrowserIconSource.Url : BrowserIconSource.LocalImage,
                Path = source
            }, image, name, Dimensions(image), source);
            if (fromUrl) urlChoice = choice;
            else localChoice = choice;
            Selection = choice;
        }
        finally
        {
            if (!isClosed) IsBusy = false;
        }
    }

    public void Close() => isClosed = true;

    private static bool IsWebUrl(string source) =>
        Uri.TryCreate(source, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

    private static string Dimensions(BitmapImage image) => $"{image.PixelWidth} × {image.PixelHeight} pixels";
}
