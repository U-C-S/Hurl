using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hurl.BrowserSelector.Models
{
    public sealed class CurrentLink : INotifyPropertyChanged
    {
        private CurrentLink(string Url)
        {
            this.Url = Url;
        }
        private string _url;

        private string Url
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

        private static CurrentLink _instance = null;

        public static string Value
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CurrentLink(string.Empty);
                }
                return _instance.Url;
            }
            set
            {
                if (_instance == null)
                {
                    _instance = new CurrentLink(value);
                }
                _instance.Url = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
