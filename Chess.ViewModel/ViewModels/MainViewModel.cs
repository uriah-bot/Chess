using Chess.ViewModel.Stores;
using System.ComponentModel;

namespace Chess.ViewModel
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly INavigationStore _navigationStore;
        public MainViewModel(INavigationStore navigationStore, LoginViewModel lvm)
        {
            _navigationStore = navigationStore;

            _navigationStore.PropertyChanged += NavigationStore_PropertyChanged;

            _navigationStore.CurrentViewModel = lvm;
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public void Dispose()
        {
            _navigationStore.PropertyChanged -= NavigationStore_PropertyChanged;
        }

        private void NavigationStore_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_navigationStore.CurrentViewModel))
            {
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
    }
}
