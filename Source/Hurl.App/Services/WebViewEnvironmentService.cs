using Hurl.App.Services.Interfaces;
using Hurl.Library;
using Hurl.Library.Models;
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Hurl.App.Services;

public class WebViewEnvironmentService(ISettingsService settingsService) : IWebViewEnvironmentService
{
    private readonly Lazy<Task<CoreWebView2Environment>> environmentTask = new(() => CreateEnvironmentAsync(settingsService));

    public Task<CoreWebView2Environment> GetEnvironmentAsync()
    {
        return environmentTask.Value;
    }

    private static async Task<CoreWebView2Environment> CreateEnvironmentAsync(ISettingsService settingsService)
    {
        Settings settings = settingsService.LoadSettings();
        QuickViewSettings quickView = settings.QuickView ?? new QuickViewSettings();
        CoreWebView2EnvironmentOptions options = new()
        {
            ScrollBarStyle = CoreWebView2ScrollbarStyle.FluentOverlay,
            AdditionalBrowserArguments = quickView.AdditionalBrowserArguments ?? string.Empty,
            AreBrowserExtensionsEnabled = quickView.BrowserExtensionsEnabled
        };

        Directory.CreateDirectory(Constants.APP_SETTINGS_DIR);
        return await CoreWebView2Environment.CreateWithOptionsAsync(null, Constants.APP_SETTINGS_DIR, options);
    }
}
