using Hurl.BrowserSelector.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hurl.Library.Models;
using Hurl.Library.Extensions;

namespace Hurl.BrowserSelector.Pages.ViewModels;

internal class TimedDefaultViewModel
{
    public IEnumerable<(Browser, int)> Browsers { get; } = SettingsGlobal.Value.Browsers.WithIndex();

    public int? CurrentSelectedIndex { get; set; }

    public bool[] CurrentToggleStatus { get; set; }
    
    public TimedDefaultViewModel()
    {

    }
}
