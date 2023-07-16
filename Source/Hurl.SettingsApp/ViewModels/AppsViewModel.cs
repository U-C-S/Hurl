using Hurl.Library.Models;
using System.Collections.Generic;

namespace Hurl.SettingsApp.ViewModels
{
    public class AppsViewModel
    {
        private List<Browser> jsonData { get; set; }

        public AppsViewModel()
        {
            jsonData = State.Settings.GetBrowsers();
        }

        public List<Browser> BrowsersList { get => jsonData; }
    }
}
