using System.ComponentModel;

namespace Chess.ViewModel.Stores
{
    public interface INavigationStore : INotifyPropertyChanged
    {
        ViewModelBase CurrentViewModel { get; set; }
    }

    public class NavigationStore : INavigationStore
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel is IDisposable oldVm)
                {
                    oldVm.Dispose();
                }

                _currentViewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentViewModel)));
            }
        }
    }
}
