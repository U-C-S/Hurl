using Hurl.Library.Models;
using System.Collections.ObjectModel;

namespace Hurl.Settings.ViewModels;

internal class BrowsersPageViewModel
{
    public ObservableCollection<Browser> Browsers { get; set; }

    public BrowsersPageViewModel()
    {
        Browsers = new(State.Settings.Browsers);
    }
}
