using Hurl.Library.Models;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace Hurl.Settings.ViewModels;

internal class BrowsersPageViewModel
{
    public ObservableCollection<Browser> Browsers { get; set; }

    public BrowsersPageViewModel(IOptions<Library.Models.Settings> settings)
    {
        Browsers = new(settings.Value.Browsers);
    }

    public void RefreshBrowserList()
    {
        State.Settings.RefreshBrowsers();
        Refresh();
    }

    public void Refresh()
    {
        Browsers.Clear();
        State.Settings.Browsers.ForEach(browser => Browsers.Add(browser));
    }
}
