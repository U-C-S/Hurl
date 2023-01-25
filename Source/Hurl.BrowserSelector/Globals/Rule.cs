using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hurl.BrowserSelector.Globals
{
    internal class Rule
    {
        private static Rule _instance = null;

        private string _rule;

        public string Rulee
        {
            get => _rule;
            set
            {
                if (value != _rule)
                {
                    _rule = value;
                    OnPropertyChanged();
                }
            }
        }

        public static string Value
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Rule();
                }
                return _instance.Rulee;
            }
            set
            {
                if (_instance == null)
                {
                    _instance = new Rule();
                }
                _instance.Rulee = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
