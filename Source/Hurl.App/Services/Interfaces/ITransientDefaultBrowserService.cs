using Hurl.Library.Models;
using System;

namespace Hurl.App.Services.Interfaces;

public interface ITransientDefaultBrowserService
{
    void Start(Browser browser, TimeSpan duration, Guid? alternateLaunchId = null);

    /// <returns>True if extended; false if there is no active selection.</returns>
    bool AddFifteenMinutes();
}
