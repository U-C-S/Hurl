using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hurl.BrowserSelector.Globals
{
    public sealed class UriGlobal : INotifyPropertyChanged
    {
        private UriGlobal(string Url)
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

        private int _ruleType = 0;

        private int RuleType
        {
            get => _ruleType;
            set
            {
                if (value != _ruleType)
                {
                    _ruleType = value;
                    OnPropertyChanged();
                }
            }
        }

        private static UriGlobal _instance = null;

        public static string Value
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UriGlobal(string.Empty);
                }
                return _instance.Url;
            }
            set
            {
                if (_instance == null)
                {
                    _instance = new UriGlobal(value);
                }
                _instance.Url = value;
            }
        }

        public static int UrlOpenType
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UriGlobal(string.Empty);
                }
                return _instance.RuleType;
            }
            set
            {
                if (_instance == null)
                {
                    _instance = new UriGlobal(string.Empty);
                }
                _instance.RuleType = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
