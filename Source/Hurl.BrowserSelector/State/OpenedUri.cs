using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hurl.BrowserSelector.State
{
    public sealed class OpenedUri : INotifyPropertyChanged
    {
        private OpenedUri(string Url) => this.Url = Url;
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

        private static OpenedUri? _instance;

        public static string Value
        {
            get
            {
                _instance ??= new OpenedUri(string.Empty);
                return _instance.Url;
            }
            set
            {
                _instance ??= new OpenedUri(value);
                _instance.Url = value;
            }
        }

        public static void Set(string url) => Value = url;

        public static void Clear() => Value = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
