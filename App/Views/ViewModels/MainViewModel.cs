using Hurl.BrowserSelector.Models;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public CurrentLink currentLink { get; set; }

        public BaseViewModel viewModel { get; set; }

        public MainViewModel(string Url)
        {
            currentLink = new CurrentLink(Url);
            viewModel = new BrowserListViewModel(currentLink);
        }
    }
}
