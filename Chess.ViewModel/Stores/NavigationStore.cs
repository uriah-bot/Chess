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
                _currentViewModel?.Dispose();

                _currentViewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentViewModel)));
            }
        }
    }
}
