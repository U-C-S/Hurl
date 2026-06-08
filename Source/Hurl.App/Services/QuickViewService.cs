using Hurl.Library.Models;
using Hurl.App.Helpers;
using Hurl.App.Pages;
using Hurl.App.Services.Interfaces;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Hurl.App.Services;

public sealed class QuickViewService(
    ISettingsService settingsService,
    IWebViewEnvironmentService webViewEnvironmentService) : IQuickViewService
{
    private readonly List<QuickViewWindow> quickViewWindows = [];

    public bool IsQuickViewEnabled
    {
        get
        {
            QuickViewSettings quickView = LoadQuickViewSettings();
            return quickView.Enabled;
        }
    }

    public bool TryOpen(string? url)
    {
        Settings settings = settingsService.LoadSettings();
        QuickViewSettings quickView = settings.QuickView ?? new QuickViewSettings();

        if (!quickView.Enabled || string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        try
        {
            List<Browser> browsers = settings.Browsers?
                .Where(browser => !browser.Hidden)
                .ToList() ?? [];

            return quickView.LaunchMode switch
            {
                QuickViewLaunchMode.Browser => TryOpenBrowser(url!, browsers, quickView),
                _ => TryOpenWebView(url!, browsers, quickView)
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    public bool TryOpenIfModifierKeyActivated(string? url)
    {
        bool isAltDown = KeyboardState.IsAltKeyDown();
        bool isCtrlDown = KeyboardState.IsCtrlKeyDown();

        QuickViewSettings quickView = LoadQuickViewSettings();
        if (!quickView.Enabled)
        {
            return false;
        }

        var result = quickView.ModifierKeys switch
        {
            QuickViewModifierKeys.CtrlAlt => isAltDown && isCtrlDown,
            QuickViewModifierKeys.Ctrl => isCtrlDown && !isAltDown,
            _ => isAltDown && !isCtrlDown
        };

        return result && TryOpen(url);
    }

    private void QuickViewWindow_Closed(object sender, WindowEventArgs args)
    {
        if (sender is QuickViewWindow quickViewWindow)
        {
            quickViewWindow.Closed -= QuickViewWindow_Closed;
            quickViewWindows.Remove(quickViewWindow);
        }
    }

    private QuickViewSettings LoadQuickViewSettings()
    {
        Settings settings = settingsService.LoadSettings();
        return settings.QuickView ?? new QuickViewSettings();
    }

    private bool TryOpenWebView(string url, List<Browser> browsers, QuickViewSettings quickView)
    {
        if (!QuickViewWindow.CanQuickView(url))
        {
            return false;
        }

        QuickViewWindow quickViewWindow = new(url, browsers, quickView, webViewEnvironmentService);
        quickViewWindow.Closed += QuickViewWindow_Closed;
        quickViewWindows.Add(quickViewWindow);
        quickViewWindow.Activate();
        return true;
    }

    private bool TryOpenBrowser(string url, List<Browser> browsers, QuickViewSettings quickView)
    {
        Browser? browser = browsers.FirstOrDefault(
            browser => string.Equals(browser.Name, quickView.BrowserName, StringComparison.OrdinalIgnoreCase));

        if (browser is null)
        {
            return false;
        }

        UriLauncher.ResolveAutomatically(url, browser, quickView.AlternateLaunchIndex);
        return true;
    }
}
