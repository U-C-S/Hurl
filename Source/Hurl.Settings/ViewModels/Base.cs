using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hurl.Settings.ViewModels
{
    public class Base : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
