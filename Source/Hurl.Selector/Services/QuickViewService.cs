using Hurl.Library.Models;
using Hurl.Selector.Pages;
using Hurl.Selector.Services.Interfaces;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Hurl.Selector.Services;

public sealed class QuickViewService(
    ISettingsService settingsService,
    IWebViewEnvironmentService webViewEnvironmentService) : IQuickViewService
{
    private readonly List<QuickViewWindow> quickViewWindows = [];

    public bool TryOpen(string? url)
    {
        if (!QuickViewWindow.CanQuickView(url))
        {
            return false;
        }

        try
        {
            Settings settings = settingsService.LoadSettings();
            List<Browser> browsers = settings.Browsers?
                .Where(browser => !browser.Hidden)
                .ToList() ?? [];

            QuickViewWindow quickViewWindow = new(url!, browsers, webViewEnvironmentService);
            quickViewWindow.Closed += QuickViewWindow_Closed;
            quickViewWindows.Add(quickViewWindow);
            quickViewWindow.Activate();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    private void QuickViewWindow_Closed(object sender, WindowEventArgs args)
    {
        if (sender is QuickViewWindow quickViewWindow)
        {
            quickViewWindow.Closed -= QuickViewWindow_Closed;
            quickViewWindows.Remove(quickViewWindow);
        }
    }
}
