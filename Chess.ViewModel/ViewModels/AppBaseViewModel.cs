using Chess.Model;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Chess.ViewModel
{
    public class AppBaseViewModel : ViewModelBase
    {
        private readonly IUserStore _userStore;
        private readonly INavigationStore _navigationStore;
        private readonly INavigationService _navigationService;
        public AppBaseViewModel(IUserStore userStore, INavigationService navigationService, INavigationStore navigationStore, HomeViewModel hvm)
        {
            _navigationService = navigationService;
            _userStore = userStore;
            _userStore.CurrentUserChanged += OnUserChanged;
            _navigationStore = navigationStore;
            _navigationStore.PropertyChanged += NavigationStore_PropertyChanged;

            NavigateToHomeCommand = new NavigateCommand<HomeViewModel>(_navigationService);
            NavigateToStatsCommand = new NavigateCommand<StatsViewModel>(_navigationService);
            //NavigateToAdvancedSettingsCommand = new NavigateCommand(_navigationService, typeof(AdvancedSettingsViewModel));
            NavigateToSettingsCommand = new NavigateCommand<SettingsViewModel>(_navigationService);
            NavigateToHelpCommand = new NavigateCommand<HelpViewModel>(_navigationService);
            _navigationStore.CurrentViewModel = hvm;
        }

        public ICommand NavigateToHomeCommand { get; }
        public ICommand NavigateToStatsCommand { get; }
        public ICommand NavigateToAdvancedSettingsCommand { get; }
        public ICommand NavigateToSettingsCommand { get; }
        public ICommand NavigateToHelpCommand { get; }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public string Username => _userStore.CurrentUser?.Username ?? "Stranger";

        public UserRole Role => _userStore.CurrentUser?.Role ?? UserRole.User;

        public string EloText => $"Elo: {_userStore.CurrentUser?.Elo ?? -1}";

        private void NavigationStore_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_navigationStore.CurrentViewModel))
            {
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public override void Dispose() // might stay in memory even when in a different view
        {
            _userStore.CurrentUserChanged -= OnUserChanged;
            _navigationStore.PropertyChanged -= NavigationStore_PropertyChanged;

            base.Dispose();
        }

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(EloText));
        }
    }
}
