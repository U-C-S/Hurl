using Hurl.BrowserSelector.Helpers;
using Hurl.BrowserSelector.Models;
using System.IO;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public Settings settings = SettingsFile.GetSettings();

        public AppAutoSettings RuntimeSettings = JsonOperations.FromJsonToModel<AppAutoSettings>(Path.Combine(Constants.APP_SETTINGS_DIR, "runtime.json"));

        public BaseViewModel viewModel
        {
            get
            {
                return new BrowserListViewModel(settings);
            }
        }
    }
}
