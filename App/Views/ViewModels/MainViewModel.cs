using Hurl.BrowserSelector.Globals;

namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public string Url
        {
            get
            {
                
                if (string.IsNullOrEmpty(CurrentLink.Value))
                {
                    return "No Url Opened";
                }
                return CurrentLink.Value;
            }
        }

        public BaseViewModel viewModel { get; set; }

        public MainViewModel()
        {
            viewModel = new BrowserListViewModel();
        }
    }
}
