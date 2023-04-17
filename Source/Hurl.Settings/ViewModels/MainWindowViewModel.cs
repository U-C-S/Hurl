using Hurl.Library;

namespace Hurl.Settings.ViewModels
{
    internal class MainWindowViewModel : ViewModels.Base
    {
        private Hurl.Library.Models.Settings _Settings = SettingsFile.GetSettings();

        public Hurl.Library.Models.Settings SettingsObj
        {
            get { return _Settings; }
            set
            {
                _Settings = value;
                OnPropertyChanged();
            }
        }
    }
}
