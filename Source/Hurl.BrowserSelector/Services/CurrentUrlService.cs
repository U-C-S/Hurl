using CommunityToolkit.Mvvm.ComponentModel;

namespace Hurl.BrowserSelector.Services;

internal partial class CurrentUrlService: ObservableObject
{
    [ObservableProperty]
    public string url = string.Empty;

    public string Get() => Url;

    public void Set(string url)
    {
        if (Url == url)
            return;
        else 
            Url = url.Trim();
    }

    public void Clear() => Url = string.Empty;
}

