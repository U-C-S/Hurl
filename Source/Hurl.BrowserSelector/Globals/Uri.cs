﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hurl.BrowserSelector.Globals
{
    public sealed class UriGlobal : INotifyPropertyChanged
    {
        private UriGlobal(string Url) => this.Url = Url;
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

        private static UriGlobal? _instance;

        public static string Value
        {
            get
            {
                _instance ??= new UriGlobal(string.Empty);
                return _instance.Url;
            }
            set
            {
                _instance ??= new UriGlobal(value);
                _instance.Url = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
