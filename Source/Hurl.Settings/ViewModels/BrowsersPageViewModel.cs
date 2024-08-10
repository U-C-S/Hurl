using Hurl.Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.Settings.ViewModels;

internal class BrowsersPageViewModel
{
    public ObservableCollection<Browser> Browsers { get; set; }

    public BrowsersPageViewModel()
    {
        Browsers = new(State.Settings.Browsers);
    }
}
