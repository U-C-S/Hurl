using CommunityToolkit.Mvvm.ComponentModel;
using Hurl.Library.Models;
using Hurl.Settings.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hurl.Settings.ViewModels;

public partial class QuickViewPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial QuickViewSettings QuickView { get; set; }

    public ObservableCollection<QuickViewBrowserLaunchOption> BrowserLaunchOptions { get; } = [];

    private readonly ISettingsService settingsService;

    public QuickViewPageViewModel(IOptionsMonitor<Library.Models.Settings> settings, ISettingsService settingsService)
    {
        QuickView = settings.CurrentValue.QuickView ?? new QuickViewSettings();
        LoadBrowserLaunchOptions(settings.CurrentValue.Browsers);
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

    public int Option_BrowserLaunchTarget
    {
        get
        {
            int selectedIndex = BrowserLaunchOptions
                .Select((option, index) => new { option, index })
                .Where(item => item.option.Matches(QuickView.BrowserName, QuickView.AlternateLaunchIndex))
                .Select(item => item.index)
                .FirstOrDefault(-1);

            return selectedIndex;
        }
        set
        {
            if (value < 0 || value >= BrowserLaunchOptions.Count)
            {
                return;
            }

            QuickViewBrowserLaunchOption option = BrowserLaunchOptions[value];
            if (option.Matches(QuickView.BrowserName, QuickView.AlternateLaunchIndex))
            {
                return;
            }

            QuickView.BrowserName = option.BrowserName;
            QuickView.AlternateLaunchIndex = option.AlternateLaunchIndex;
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
                browser.Name,
                null));

            if (browser.AlternateLaunches is not { Count: > 0 })
            {
                continue;
            }

            for (int index = 0; index < browser.AlternateLaunches.Count; index++)
            {
                AlternateLaunch alternateLaunch = browser.AlternateLaunches[index];
                BrowserLaunchOptions.Add(new QuickViewBrowserLaunchOption(
                    $"{browser.Name} - {alternateLaunch.ItemName}",
                    browser.Name,
                    index));
            }
        }
    }

    private void EnsureBrowserLaunchTarget()
    {
        if (QuickView.LaunchMode != QuickViewLaunchMode.Browser
            || BrowserLaunchOptions.Count == 0
            || Option_BrowserLaunchTarget >= 0)
        {
            return;
        }

        QuickViewBrowserLaunchOption option = BrowserLaunchOptions[0];
        QuickView.BrowserName = option.BrowserName;
        QuickView.AlternateLaunchIndex = option.AlternateLaunchIndex;
    }
}

public sealed record QuickViewBrowserLaunchOption(
    string DisplayName,
    string BrowserName,
    int? AlternateLaunchIndex)
{
    public bool Matches(string browserName, int? alternateLaunchIndex)
    {
        return string.Equals(BrowserName, browserName, StringComparison.OrdinalIgnoreCase)
            && AlternateLaunchIndex == alternateLaunchIndex;
    }
}
