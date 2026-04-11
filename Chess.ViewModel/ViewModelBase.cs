using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chess.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        // Clear event listeners
        public virtual void Dispose()
        {
            PropertyChanged = null;
        }

        // CallerMemberName allows for auto assignning to the caller's name e.g "Username" property.
        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
