using Hurl.App.Services.Interfaces;
using Hurl.Library.Models;
using System;
using System.Linq;

namespace Hurl.App.Services;

public sealed class TransientDefaultBrowserService(
    IAppStateService appStateService,
    TimeProvider timeProvider) : ITransientDefaultBrowserService
{
    public void Start(Browser browser, TimeSpan duration, Guid? alternateLaunchId = null)
    {
        ArgumentNullException.ThrowIfNull(browser);
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be positive.");
        }

        AlternateLaunch? alternateLaunch = null;
        if (alternateLaunchId is Guid id)
        {
            alternateLaunch = browser.AlternateLaunches?.FirstOrDefault(launch => launch.Id == id)
                ?? throw new ArgumentException("Alternate launch profile does not exist.", nameof(alternateLaunchId));
        }

        DateTime selectedAt = timeProvider.GetUtcNow().UtcDateTime;
        appStateService.UpdateTransientDefault(new TransientDefaultConfig
        {
            Browser = new TransientBrowser(
                browser.Id,
                alternateLaunch?.Id,
                browser.Name,
                browser.ExePath,
                alternateLaunch is null ? browser.LaunchArgs ?? string.Empty : alternateLaunch.LaunchArgs),
            SelectedAt = selectedAt,
            ValidTill = selectedAt.Add(duration)
        });
    }

    public bool AddFifteenMinutes()
    {
        TransientDefaultConfig? current = appStateService.LoadState().TransientDefaultConfig;
        if (current is null || current.ValidTill <= timeProvider.GetUtcNow().UtcDateTime)
        {
            return false;
        }

        appStateService.UpdateTransientDefault(new TransientDefaultConfig
        {
            Browser = current.Browser,
            SelectedAt = current.SelectedAt,
            ValidTill = current.ValidTill.AddMinutes(15)
        });

        return true;
    }
}
