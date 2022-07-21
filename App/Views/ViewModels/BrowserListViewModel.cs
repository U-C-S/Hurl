using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class BrowserListViewModel : BaseViewModel
    {
        public List<Browser> Browsers { get; set; }
        public CurrentLink Link { get; set; }

        public BrowserListViewModel(CurrentLink _link)
        {
            Link = _link;
            LoadBrowsers();
        }

        private void LoadBrowsers()
        {
#if DEBUG
            Stopwatch sw = new();
            sw.Start();
#endif
            try
            {
                Browsers = GetBrowsers.FromSettingsFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Shutdown();
                return;
            }
#if DEBUG
            sw.Stop();
            Debug.WriteLine("---------" + sw.ElapsedMilliseconds.ToString());
#endif
        }
    }
}
