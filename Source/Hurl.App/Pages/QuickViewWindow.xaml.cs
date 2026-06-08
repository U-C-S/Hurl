using Hurl.Library.Models;
using Hurl.App.Helpers;
using Hurl.App.Services.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using WinUIEx;

namespace Hurl.App.Pages;

public sealed partial class QuickViewWindow : Window
{
    private const double DefaultWidth = 1080;
    private const double DefaultHeight = 720;
    private const double ToolbarPadding = 8;
    private const double CaptionButtonGap = 8;
    private static readonly string[] SupportedSchemes = ["http", "https"];

    private readonly IReadOnlyList<Browser> browsers;
    private readonly QuickViewSettings quickViewSettings;
    private readonly IWebViewEnvironmentService webViewEnvironmentService;
    private readonly WebView2 quickWebView;
    private bool isWebViewInitialized;
    private Uri? pendingNavigationUri;

    public QuickViewWindow(
        string url,
        List<Browser> browsers,
        QuickViewSettings quickViewSettings,
        IWebViewEnvironmentService webViewEnvironmentService)
    {
        this.browsers = browsers;
        this.quickViewSettings = quickViewSettings;
        this.webViewEnvironmentService = webViewEnvironmentService;

        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
        ToolbarBorder.Loaded += ToolbarBorder_Loaded;
        this.SetWindowSize(DefaultWidth, DefaultHeight);

        quickWebView = new WebView2();
        quickWebView.CoreWebView2Initialized += QuickWebView_CoreWebView2Initialized;
        quickWebView.NavigationStarting += QuickWebView_NavigationStarting;
        quickWebView.NavigationCompleted += QuickWebView_NavigationCompleted;
        WebViewHost.Children.Add(quickWebView);

        Closed += QuickViewWindow_Closed;

        ConfigureOpenInFlyout();
        _ = InitializeWebViewAsync(url);
    }

    #region Lifecycle Events
    private void QuickViewWindow_Closed(object sender, WindowEventArgs args)
    {
        ToolbarBorder.Loaded -= ToolbarBorder_Loaded;

        if (quickWebView.CoreWebView2 is not null)
        {
            quickWebView.CoreWebView2.NewWindowRequested -= CoreWebView2_NewWindowRequested;
        }

        quickWebView.CoreWebView2Initialized -= QuickWebView_CoreWebView2Initialized;
        quickWebView.NavigationStarting -= QuickWebView_NavigationStarting;
        quickWebView.NavigationCompleted -= QuickWebView_NavigationCompleted;
    }

    private void ToolbarBorder_Loaded(object sender, RoutedEventArgs e)
    {
        double scale = ToolbarBorder.XamlRoot?.RasterizationScale ?? 1;
        double captionInset = AppWindow.TitleBar.RightInset / scale;
        double rightPadding = Math.Max(ToolbarPadding, captionInset + CaptionButtonGap);

        ToolbarBorder.Padding = new Thickness(
            ToolbarPadding,
            ToolbarPadding,
            rightPadding,
            ToolbarPadding);
    }

    private async Task InitializeWebViewAsync(string? initialUrl)
    {
        Navigate(initialUrl);

        try
        {
            CoreWebView2Environment environment = await webViewEnvironmentService.GetEnvironmentAsync();
            await quickWebView.EnsureCoreWebView2Async(environment);

            isWebViewInitialized = true;
            if (pendingNavigationUri is not null)
            {
                Uri uri = pendingNavigationUri;
                pendingNavigationUri = null;
                Navigate(uri.AbsoluteUri);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            SetLoading(false);
        }
    }

    private void ConfigureOpenInFlyout()
    {
        MenuFlyout flyout = new();

        foreach (Browser browser in browsers)
        {
            MenuFlyoutItem item = new()
            {
                Text = browser.Name,
                Tag = browser
            };
            item.Click += OpenInBrowser_Click;
            flyout.Items.Add(item);
        }

        OpenInButton.Flyout = flyout;
        OpenInButton.IsEnabled = flyout.Items.Count > 0;
    }
    #endregion

    #region UI Event Handlers
    private void OpenInBrowser_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuFlyoutItem { Tag: Browser browser })
        {
            return;
        }

        try
        {
            UriLauncher.ResolveAutomatically(GetCurrentUrl(), browser, null);
            Close();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (quickWebView.CanGoBack)
        {
            quickWebView.GoBack();
        }
    }

    private void ForwardButton_Click(object sender, RoutedEventArgs e)
    {
        if (quickWebView.CanGoForward)
        {
            quickWebView.GoForward();
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        if (!isWebViewInitialized)
        {
            return;
        }

        quickWebView.Reload();
    }

    private void AddressTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key != VirtualKey.Enter)
        {
            return;
        }

        Navigate(AddressTextBox.Text);
        e.Handled = true;
    }
    #endregion

    #region WebView Event Handlers
    private void QuickWebView_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
    {
        if (args.Exception is not null)
        {
            Debug.WriteLine(args.Exception);
            SetLoading(false);
            return;
        }

        if (sender.CoreWebView2 is not null)
        {
            ApplyTrackingPrevention(sender.CoreWebView2);
            sender.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }
    }

    private void CoreWebView2_NewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
    {
        if (TryCreateQuickViewUri(args.Uri, out var uri))
        {
            args.Handled = true;
            Navigate(uri.AbsoluteUri);
        }
    }

    private void QuickWebView_NavigationStarting(WebView2 sender, CoreWebView2NavigationStartingEventArgs args)
    {
        AddressTextBox.Text = args.Uri;
        SetLoading(true);
        UpdateNavigationButtons();
    }

    private void QuickWebView_NavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        if (sender.Source is not null)
        {
            AddressTextBox.Text = sender.Source.AbsoluteUri;
        }

        SetLoading(false);
        UpdateNavigationButtons();
    }
    #endregion

    private void Navigate(string? url)
    {
        if (!TryCreateQuickViewUri(url, out var uri))
        {
            return;
        }

        AddressTextBox.Text = uri.AbsoluteUri;
        if (!isWebViewInitialized)
        {
            pendingNavigationUri = uri;
            return;
        }

        quickWebView.Source = uri;
    }

    private string GetCurrentUrl()
    {
        return quickWebView.Source?.AbsoluteUri ?? AddressTextBox.Text;
    }

    private void SetLoading(bool isLoading)
    {
        LoadingRing.IsActive = isLoading;
        LoadingRing.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
    }

    private void UpdateNavigationButtons()
    {
        BackButton.IsEnabled = quickWebView.CanGoBack;
        ForwardButton.IsEnabled = quickWebView.CanGoForward;
    }

    private void ApplyTrackingPrevention(CoreWebView2 coreWebView)
    {
        try
        {
            coreWebView.Profile.PreferredTrackingPreventionLevel = quickViewSettings.TrackingPrevention switch
            {
                QuickViewTrackingPreventionLevel.None => CoreWebView2TrackingPreventionLevel.None,
                QuickViewTrackingPreventionLevel.Basic => CoreWebView2TrackingPreventionLevel.Basic,
                QuickViewTrackingPreventionLevel.Strict => CoreWebView2TrackingPreventionLevel.Strict,
                _ => CoreWebView2TrackingPreventionLevel.Balanced
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    #region Utils
    public static bool CanQuickView(string? url)
    {
        return TryCreateQuickViewUri(url, out _);
    }

    private static bool TryCreateQuickViewUri(string? value, out Uri uri)
    {
        uri = null!;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        string normalized = value.Trim().Trim('"');
        if (!Uri.TryCreate(normalized, UriKind.Absolute, out var parsed)
            && !Uri.TryCreate($"https://{normalized}", UriKind.Absolute, out parsed))
        {
            return false;
        }

        if (!SupportedSchemes.Contains(parsed.Scheme, StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        uri = parsed;
        return true;
    }
    #endregion
}
