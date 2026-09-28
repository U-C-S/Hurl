using Hurl.Library.Models;
using System;

namespace Hurl.App.Services.Interfaces;

public interface ITransientDefaultBrowserService
{
    void Start(Browser browser, TimeSpan duration, Guid? alternateLaunchId = null);

    void End();

    /// <returns>The saved browser if the selection is active; otherwise null.</returns>
    TransientBrowser? GetActiveBrowser();

    /// <returns>The remaining duration, or zero if there is no active selection.</returns>
    TimeSpan GetRemainingTime();

    /// <returns>True if extended, false if there is no active selection.</returns>
    bool AddFifteenMinutes();
}
