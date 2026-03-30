using Chess.ViewModel.Stores;
using System.ComponentModel;

namespace Chess.ViewModel
{
    public class AppBaseViewModel : ViewModelBase, IDisposable
    {
        private readonly IUserStore _userStore;
        private readonly INavigationStore _navigationStore;
        public AppBaseViewModel(IUserStore userStore, INavigationStore navigationStore)
        {
            _userStore = userStore;
            _userStore.CurrentUserChanged += OnUserChanged;
            _navigationStore = navigationStore;
            _navigationStore.PropertyChanged += NavigationStore_PropertyChanged;
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public string Username => _userStore.CurrentUser?.Username ?? "Stranger";

        public int Elo => _userStore.CurrentUser?.Elo ?? -1;

        public string EloText => $"Elo: {Elo}";

        private void NavigationStore_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_navigationStore.CurrentViewModel))
            {
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public void Dispose() // might stay in memory even when in a different view
        {
            _userStore.CurrentUserChanged -= OnUserChanged;
        }

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Elo));
        }
    }
}
