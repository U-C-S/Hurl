using CommunityToolkit.Mvvm.ComponentModel;

namespace Hurl.BrowserSelector.Services;

internal partial class CurrentUrlService: ObservableObject
{
    [ObservableProperty]
    public string url = string.Empty;

    public string Get() => Url;

    public void Set(string url) => Url = url;

    public void Clear() => Url = string.Empty;
}

