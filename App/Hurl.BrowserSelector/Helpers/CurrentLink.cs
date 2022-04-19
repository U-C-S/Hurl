using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hurl.BrowserSelector.Helpers
{
    public class CurrentLink : INotifyPropertyChanged
    {
        public CurrentLink(string URL)
        {
            this._url = URL;
        }
        private string _url;

        public string Url
        {
            get => _url;
            set
            {
                if (value != _url)
                {
                    _url = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
