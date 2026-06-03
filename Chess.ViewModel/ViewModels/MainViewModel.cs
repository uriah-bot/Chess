using Chess.ViewModel.Stores;
using System.ComponentModel;

namespace Chess.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationStore _navigationStore;
        public MainViewModel(INavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.PropertyChanged += NavigationStore_PropertyChanged;
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public override void Dispose()
        {
            _navigationStore.PropertyChanged -= NavigationStore_PropertyChanged;

            base.Dispose();
        }

        // refreshes the UI (the if should be true, it's just safe-proofing)
        private void NavigationStore_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_navigationStore.CurrentViewModel))
            {
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
    }
}
