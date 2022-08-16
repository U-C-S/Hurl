namespace Hurl.BrowserSelector.Views.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public BaseViewModel viewModel { get; set; }

        public MainViewModel()
        {
            viewModel = new BrowserListViewModel();
        }
    }
}
