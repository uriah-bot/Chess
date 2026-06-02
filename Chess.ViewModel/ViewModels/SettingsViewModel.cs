using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;
using static Chess.Data.Repositories;
using Chess.Model;
using Chess.Service;
using System.Collections.ObjectModel;
using System.IO;

namespace Chess.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
		private readonly IUserStore _userStore;
		private readonly INavigationService _navigationService;
		private readonly IWindowService _windowService;
		private readonly IUserRepository _userRepo;
		private readonly IDecorStore _decorStore;
		private readonly ISettingsRepository _settingsRepo;
		private readonly IFileService _fileService;
		private readonly ICustomizableDecorManager<RadioChannelEntity> _radio;

        public SettingsViewModel(IUserStore userStore, INavigationService navigationService, IWindowService windowService, IUserRepository userRepository,
            IDecorStore decorStore, IFileService fileService, ISettingsRepository settingsRepo, ICustomizableDecorManager<RadioChannelEntity> radioChannel)
        {
            _userStore = userStore;
            _navigationService = navigationService;
            _windowService = windowService;
            _userRepo = userRepository;
            _settingsRepo = settingsRepo;
            _radio = radioChannel;
            _decorStore = decorStore;
            _fileService = fileService;

            _userStore.CurrentUserChanged += OnUserChanged;

            DeleteUserCommand = new RelayCommand(o => DeleteUserAsync(), o => CanDeleteUser());
            PlaySelectedMusicCommand = new RelayCommand(o => PlayMusicAsync(o));
            ShowPopupCommand = new RelayCommand(o => ShowPopup(o), o => _userStore.IsLoggedIn);
            AddMusicCommand = new RelayCommand(o => AddMusic(), o => _userStore.IsLoggedIn);
            SaveVolumeCommand = new RelayCommand(o => UpdateUserVolumeAsync(), o => _userStore.IsLoggedIn);
            DisplayCoordinatesCommand = new RelayCommand(async o =>
            {
                DisplayCoordinates = !DisplayCoordinates;
                await _settingsRepo.UpdateUserSettingsAsync(_userStore.CurrentUser);
            }, o => _userStore.IsLoggedIn);

            StopRadioOnMatchesCommand = new RelayCommand(async o =>
            {
                StopRadioOnMatches = !StopRadioOnMatches;
                await _settingsRepo.UpdateUserSettingsAsync(_userStore.CurrentUser);
            }, o => _userStore.IsLoggedIn);

            _ = LoadChannelsAsync();
        }

        private ObservableCollection<RadioChannelEntity> _musicChannels;
        public ObservableCollection<RadioChannelEntity> MusicChannels
        {
            get
            {
                return _musicChannels;
            }
            set
            {
                _musicChannels = value;
                _musicChannels.Where(c => c.ChannelPath == (_userStore.CurrentUser?.Settings?.CurrentSong ?? "DefaultMusic.mp3")).ToList().ForEach(c => c.IsSelected = true);
                OnPropertyChanged(nameof(MusicChannels));
            }
        }

        public string Username => _userStore.CurrentUser?.Username ?? "Stranger";

        public int Volume
        {
            get => (int)(_decorStore.CurrentVolume * 100);
            set
            {
                _decorStore.CurrentVolume = (double)value / 100;
                OnPropertyChanged(nameof(Volume));
            }
        }

        public bool StopRadioOnMatches
        {
            get
            {
                return _userStore.CurrentUser?.Settings?.StopRadioOnMatches ?? true;
            }
            set
            {
                _userStore.Update(u => u.Settings.StopRadioOnMatches = value);
                OnPropertyChanged(nameof(StopRadioOnMatches));
            }
        }

        public bool DisplayCoordinates
        {
            get
            {
                return _userStore.CurrentUser?.Settings?.DisplayCoordinates ?? false;
            }
            set
            {
                _userStore.Update(u => u.Settings.DisplayCoordinates = value);
                OnPropertyChanged(nameof(DisplayCoordinates));
            }
        }

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(Username));
        }

        public string Role => _userStore.CurrentUser?.Role.ToString() ?? "Guest";

        public ICommand DeleteUserCommand { get; }
        public ICommand PlaySelectedMusicCommand { get; }
        public ICommand ShowPopupCommand { get; }
        public ICommand AddMusicCommand { get; }
        public ICommand SaveVolumeCommand { get; }
        public ICommand DisplayCoordinatesCommand { get; }
        public ICommand StopRadioOnMatchesCommand { get; }

        private void AddMusic()
        {
			var path = _fileService.SelectFile("Add MP3", new string[] {".mp3"});

			if (path != null)
			{
                var destination = _fileService.SaveRadioFileForUser<RadioChannelEntity>(path, _userStore.CurrentUser);
				MusicChannels.Add(new RadioChannelEntity { ChannelName = Path.GetFileNameWithoutExtension(destination), ChannelPath = destination});
            }
        }

        private void ShowPopup(object o)
        {
            var property = o as string;
            _userStore.AppendingPropertyChange = property;
            _windowService.ShowDialog<AccountModificationMenuViewModel>();
		}

        private async Task LoadChannelsAsync()
        {
            await _radio.GetDefaultItemsAsync();

            if (_userStore.IsLoggedIn)
            {
			    _radio.dbEntities.AddRange(_fileService.GetUserRadioFiles(_userStore.CurrentUser));
            }

            MusicChannels = new ObservableCollection<RadioChannelEntity>(_radio.dbEntities);
        }

        private async void PlayMusicAsync(object content)
        {
			string musicName = content as string;
			if (musicName != null)
			{
				var music = MusicChannels.FirstOrDefault(m => m.ChannelName == musicName);
				if (music != null)
				{
                    _decorStore.CurrentSong = new Uri(Path.Combine(AppConstants.BASE_PATH, music.ChannelPath), UriKind.RelativeOrAbsolute);
                    if (_userStore.IsLoggedIn)
                    {
                        _userStore.Update(u => u.Settings.CurrentSong = music.ChannelPath);
                        await _settingsRepo.UpdateUserSettingsAsync(_userStore.CurrentUser);
                    }
                }
            }
		}

        private async void UpdateUserVolumeAsync()
        {
            _userStore.Update(u => u.Settings.Volume = _decorStore.CurrentVolume);
            await _settingsRepo.UpdateUserSettingsAsync(_userStore.CurrentUser);
        }

        private bool CanDeleteUser()
        {
            // there is no user logged in, nothing to delete
            if (!_userStore.IsLoggedIn)
			{
				return false;
			}

            // papa and I's accounts are sacred, we won't delete them
            if (_userStore.CurrentUser.Id == AppConstants.MY_USER_ID || _userStore.CurrentUser.Id == AppConstants.PAPA_MOR_USER_ID)
			{
				return false;
			}

            // couldn't care less about the rest of the users, delete them all
            return true;
        }

        private async void DeleteUserAsync()
        {
			await _userRepo.DeleteUserAsync(_userStore.CurrentUser);
            _userStore.CurrentUser = null;

            _navigationService.NavigateTo<LoginViewModel>();
			_windowService.SwitchWindow<MainViewModel>();
        }

        public override void Dispose()
        {
            _userStore.CurrentUserChanged -= OnUserChanged;
            base.Dispose();
        }
	}
}
