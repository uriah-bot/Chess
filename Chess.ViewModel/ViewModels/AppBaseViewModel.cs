using Chess.Model;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class AppBaseViewModel : ViewModelBase
    {
        private readonly IUserStore _userStore;
        private readonly INavigationStore _navigationStore;
        private readonly INavigationService _navigationService;
        private readonly IDecorStore _decorStore;

        public AppBaseViewModel(IUserStore userStore, INavigationService navigationService, INavigationStore navigationStore, IDecorStore decorStore, HomeViewModel hvm)
        {
            _navigationService = navigationService;
            _userStore = userStore;
            _decorStore = decorStore;
            _userStore.CurrentUserChanged += OnUserChanged;
            _navigationStore = navigationStore;
            _navigationStore.PropertyChanged += NavigationStore_PropertyChanged;
            _decorStore.CurrentSongChanged += OnSongChanged;
            _decorStore.VolumeChanged += OnVolumeChanged;

            NavigateRadioButtonCommand = new RelayCommand(o => NavigateToUserControl(o));
            _navigationStore.CurrentViewModel = hvm;

            _decorStore.CurrentSong = new Uri(Path.Combine(AppConstants.BASE_PATH, "DefaultMusic.mp3"), UriKind.RelativeOrAbsolute); //TODO: change
        }

        private void OnVolumeChanged()
        {
            OnPropertyChanged(nameof(Volume));
        }

        private void NavigateToUserControl(object o)
        {
            var viewName = o as string;

            switch (viewName)
            {
                case "Home":
                    new NavigateCommand<HomeViewModel>(_navigationService).Execute(null);
                        return;
                case "Adventure":
                    new NavigateCommand<AdventureViewModel>(_navigationService).Execute(null);
                    return;
                case "Stats":
                    new NavigateCommand<StatsViewModel>(_navigationService).Execute(null);
                    return;
                case "AdvancedSettings":
                    new NavigateCommand<AdvancedSettingsViewModel>(_navigationService).Execute(null);
                    return;
                case "Settings":
                    new NavigateCommand<SettingsViewModel>(_navigationService).Execute(null);
                    return;
                case "Help":
                    new NavigateCommand<HelpViewModel>(_navigationService).Execute(null);
                    return;
                default:
                    throw new ArgumentException("Invalid view name", nameof(o));
            }
        }

        public ICommand NavigateRadioButtonCommand { get; }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public string Username => _userStore.CurrentUser?.Username ?? "Stranger";

        public UserRole Role => _userStore.CurrentUser?.Role ?? UserRole.User;

        public string EloText => $"Elo: {_userStore.CurrentUser?.Elo ?? -1}";

        public Uri MusicUri => _decorStore.CurrentSong;

        public double Volume => _decorStore.CurrentVolume;

        private void NavigationStore_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_navigationStore.CurrentViewModel))
            {
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        private void OnSongChanged()
        {
            OnPropertyChanged(nameof(MusicUri));
        }

        public override void Dispose() // might stay in memory even when in a different view
        {
            _userStore.CurrentUserChanged -= OnUserChanged;
            _navigationStore.PropertyChanged -= NavigationStore_PropertyChanged;
            _decorStore.CurrentSongChanged -= OnSongChanged;
            _decorStore.VolumeChanged -= OnVolumeChanged;

            base.Dispose();
        }

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(EloText));
            OnPropertyChanged(nameof(Role));
        }
    }
}
