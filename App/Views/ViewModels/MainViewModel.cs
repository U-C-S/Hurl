using Hurl.BrowserSelector.Models;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(currentLink.Url))
                {
                    return "No Url Opened";
                }
                return currentLink.Url;
            }
        }
        public CurrentLink currentLink { get; set; }

        public BaseViewModel viewModel { get; set; }

        public MainViewModel(string Url)
        {
            currentLink = new CurrentLink(Url);
            viewModel = new BrowserListViewModel(currentLink);
        }
    }
}
