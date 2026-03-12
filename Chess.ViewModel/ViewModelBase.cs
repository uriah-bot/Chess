using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chess.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // CallerMemberName allows for auto assignning to the caller's name e.g "Username" property.
        protected void OnPropertyChanged( [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
