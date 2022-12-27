using Hurl.Library;
using Hurl.Library.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public Settings settings = SettingsFile.GetSettings();

        public BaseViewModel viewModel
        {
            get
            {
                return new BrowserListViewModel(settings);
            }
        }
    }
}
