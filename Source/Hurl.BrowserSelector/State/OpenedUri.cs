namespace Hurl.BrowserSelector.State
{
    public sealed class OpenedUri
    {
        private OpenedUri(string Url) => this._url = Url;
        private string _url;

        private string Url
        {
            get => _url;
            set
            {
                if (value != _url)
                {
                    _url = value;
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
    }
}
