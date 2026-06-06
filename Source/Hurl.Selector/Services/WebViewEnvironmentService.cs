using Hurl.Library;
using Hurl.Selector.Services.Interfaces;
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Hurl.Selector.Services;

public class WebViewEnvironmentService : IWebViewEnvironmentService
{
    private readonly Lazy<Task<CoreWebView2Environment>> environmentTask = new(CreateEnvironmentAsync);

    public Task<CoreWebView2Environment> GetEnvironmentAsync()
    {
        return environmentTask.Value;
    }

    private static async Task<CoreWebView2Environment> CreateEnvironmentAsync()
    {
        Directory.CreateDirectory(Constants.APP_SETTINGS_DIR);
        return await CoreWebView2Environment.CreateWithOptionsAsync(null, Constants.APP_SETTINGS_DIR, null);
    }
}
