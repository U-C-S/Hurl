using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.App.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hurl.App.ViewModels;

public partial class QuickViewPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial QuickViewSettings QuickView { get; set; }

    public ObservableCollection<QuickViewBrowserLaunchOption> BrowserLaunchOptions { get; } = [];

    private readonly ISettingsService settingsService;

    public QuickViewPageViewModel(ISettingsService settingsService)
    {
        QuickView = settingsService.LoadSettings().QuickView ?? new QuickViewSettings();
        LoadBrowserLaunchOptions(settingsService.LoadSettings().Browsers);
        EnsureBrowserLaunchTarget();
        this.settingsService = settingsService;
    }

    public bool Option_Enabled
    {
        get => QuickView.Enabled;
        set
        {
            if (QuickView.Enabled == value)
            {
                return;
            }

            QuickView.Enabled = value;
            SaveQuickView();
            OnPropertyChanged();
            OnPropertyChanged(nameof(WebViewOptionsEnabled));
            OnPropertyChanged(nameof(BrowserLaunchOptionsEnabled));
        }
    }

    public int Option_LaunchMode
    {
        get => QuickView.LaunchMode switch
        {
            QuickViewLaunchMode.Browser => 1,
            _ => 0
        };
        set
        {
            QuickViewLaunchMode launchMode = value == 1
                ? QuickViewLaunchMode.Browser
                : QuickViewLaunchMode.WebView;

            if (QuickView.LaunchMode == launchMode)
            {
                return;
            }

            QuickView.LaunchMode = launchMode;
            EnsureBrowserLaunchTarget();
            SaveQuickView();
            OnPropertyChanged();
            OnPropertyChanged(nameof(WebViewOptionsEnabled));
            OnPropertyChanged(nameof(BrowserLaunchOptionsEnabled));
            OnPropertyChanged(nameof(Option_BrowserLaunchTarget));
        }
    }

    public int Option_ModifierKeys
    {
        get => QuickView.ModifierKeys switch
        {
            QuickViewModifierKeys.CtrlAlt => 1,
            QuickViewModifierKeys.Ctrl => 2,
            _ => 0
        };
        set
        {
            QuickViewModifierKeys modifierKeys = value switch
            {
                1 => QuickViewModifierKeys.CtrlAlt,
                2 => QuickViewModifierKeys.Ctrl,
                _ => QuickViewModifierKeys.Alt
            };

            if (QuickView.ModifierKeys == modifierKeys)
            {
                return;
            }

            QuickView.ModifierKeys = modifierKeys;
            SaveQuickView();
            OnPropertyChanged();
        }
    }

    public QuickViewBrowserLaunchOption? Option_BrowserLaunchTarget
    {
        get => BrowserLaunchOptions.FirstOrDefault(option =>
            option.BrowserId == QuickView.BrowserId && option.AlternateLaunchId == QuickView.AlternateLaunchId);
        set
        {
            if (value is null || value == Option_BrowserLaunchTarget)
            {
                return;
            }

            QuickView.BrowserId = value.BrowserId;
            QuickView.AlternateLaunchId = value.AlternateLaunchId;
            SaveQuickView();
            OnPropertyChanged();
        }
    }
    public string Option_AdditionalBrowserArguments
    {
        get => QuickView.AdditionalBrowserArguments;
        set
        {
            value ??= string.Empty;
            if (QuickView.AdditionalBrowserArguments == value)
            {
                return;
            }

            QuickView.AdditionalBrowserArguments = value;
            SaveQuickView();
            OnPropertyChanged();
        }
    }

    public bool Option_BrowserExtensionsEnabled
    {
        get => QuickView.BrowserExtensionsEnabled;
        set
        {
            if (QuickView.BrowserExtensionsEnabled == value)
            {
                return;
            }

            QuickView.BrowserExtensionsEnabled = value;
            SaveQuickView();
            OnPropertyChanged();
        }
    }

    public int Option_TrackingPrevention
    {
        get => QuickView.TrackingPrevention switch
        {
            QuickViewTrackingPreventionLevel.None => 0,
            QuickViewTrackingPreventionLevel.Basic => 1,
            QuickViewTrackingPreventionLevel.Strict => 3,
            _ => 2
        };
        set
        {
            QuickViewTrackingPreventionLevel trackingPrevention = value switch
            {
                0 => QuickViewTrackingPreventionLevel.None,
                1 => QuickViewTrackingPreventionLevel.Basic,
                3 => QuickViewTrackingPreventionLevel.Strict,
                _ => QuickViewTrackingPreventionLevel.Balanced
            };

            if (QuickView.TrackingPrevention == trackingPrevention)
            {
                return;
            }

            QuickView.TrackingPrevention = trackingPrevention;
            SaveQuickView();
            OnPropertyChanged();
        }
    }

    public bool WebViewOptionsEnabled => Option_Enabled
        && QuickView.LaunchMode == QuickViewLaunchMode.WebView;

    public bool BrowserLaunchOptionsEnabled => Option_Enabled
        && QuickView.LaunchMode == QuickViewLaunchMode.Browser;

    private void SaveQuickView()
    {
        settingsService.UpdateQuickView(QuickView);
    }

    private void LoadBrowserLaunchOptions(IEnumerable<Browser> browsers)
    {
        foreach (Browser browser in browsers.Where(browser => !browser.Hidden))
        {
            BrowserLaunchOptions.Add(new QuickViewBrowserLaunchOption(
                $"{browser.Name} - Default",
                browser.Id,
                null));

            if (browser.AlternateLaunches is not { Count: > 0 })
            {
                continue;
            }

            foreach (AlternateLaunch alternateLaunch in browser.AlternateLaunches)
            {
                BrowserLaunchOptions.Add(new QuickViewBrowserLaunchOption(
                    $"{browser.Name} - {alternateLaunch.ItemName}",
                    browser.Id,
                    alternateLaunch.Id));
            }
        }
    }

    private void EnsureBrowserLaunchTarget()
    {
        if (QuickView.LaunchMode != QuickViewLaunchMode.Browser
            || BrowserLaunchOptions.Count == 0
            || Option_BrowserLaunchTarget is not null)
        {
            return;
        }

        QuickViewBrowserLaunchOption option = BrowserLaunchOptions[0];
        QuickView.BrowserId = option.BrowserId;
        QuickView.AlternateLaunchId = option.AlternateLaunchId;
    }
}

public sealed record QuickViewBrowserLaunchOption(
    string DisplayName,
    Guid BrowserId,
    Guid? AlternateLaunchId);
