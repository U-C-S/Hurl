using Hurl.Library.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.BrowserSelector.Services
{
    internal class SettingsService(IOptionsMonitor<Settings> settings)
    {
        private readonly IOptionsMonitor<Settings> _settings = settings;

        public Settings GetSettings() => _settings.CurrentValue;
        public void UpdateSettings(Settings settings)
        {
            //_settings.Update(settings);
        }
    }
}
