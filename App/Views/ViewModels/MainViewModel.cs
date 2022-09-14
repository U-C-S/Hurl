using Hurl.BrowserSelector.Models;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public BaseViewModel viewModel { get; set; }

        public MainViewModel(Settings settings)
        {
            viewModel = new BrowserListViewModel(settings);
        }
    }
}
