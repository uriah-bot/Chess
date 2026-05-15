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
		private readonly ICustomizableDecorManager<RadioChannelEntity> _radioChannel;

        public SettingsViewModel(IUserStore userStore, INavigationService navigationService, IWindowService windowService,IUserRepository userRepository,
            IDecorStore decorStore, IFileService fileService, ISettingsRepository settingsRepo, ICustomizableDecorManager<RadioChannelEntity> radioChannel)
        {
            _userStore = userStore;
			_navigationService = navigationService;
			_windowService = windowService;
			_userRepo = userRepository;
            _settingsRepo = settingsRepo;
			_radioChannel = radioChannel;
			_decorStore = decorStore;
			_fileService = fileService;

            _userStore.CurrentUserChanged += OnUserChanged;

			DeleteUserCommand = new RelayCommand(o => DeleteUserAsync(), o => CanDeleteUser());
			PlaySelectedMusicCommand = new RelayCommand(o => PlayMusicAsync(o));
            ShowPopupCommand = new RelayCommand(o => ShowPopup(o), o => _userStore.IsLoggedIn);
			AddMusicCommand = new RelayCommand(o => AddMusic(), o => _userStore.IsLoggedIn);
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

        private bool _muteRadioDuringGame;
        public bool MuteRadioDuringGame
        {
            get
            {
                return _muteRadioDuringGame;
            }
            set
            {
                _muteRadioDuringGame = value;
                OnPropertyChanged(nameof(MuteRadioDuringGame));
            }
        }

        private bool _playSoundOnMove;
        public bool PLaySoundOnMove
        {
            get
            {
                return _playSoundOnMove;
            }
            set
            {
                _playSoundOnMove = value;
                OnPropertyChanged(nameof(PLaySoundOnMove));
            }
        }

        private bool _showHighlights;
        public bool ShowHighlights
        {
            get
            {
                return _showHighlights;
            }
            set
            {
                _showHighlights = value;
                OnPropertyChanged(nameof(ShowHighlights));
            }
        }

        private bool _displayCoordinates;
        public bool DisplayCoordinates
        {
            get
            {
                return _displayCoordinates;
            }
            set
            {
                _displayCoordinates = value;
                OnPropertyChanged(nameof(DisplayCoordinates));
            }
        }

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Role));
        }

        public string Role => _userStore.CurrentUser?.Role.ToString() ?? "Guest";

        public ICommand DeleteUserCommand { get; }
        public ICommand PlaySelectedMusicCommand { get; }
        public ICommand ShowPopupCommand { get; }
        public ICommand AddMusicCommand { get; }
        public ICommand PlaySoundEffectOnMoveCommand { get; }

        private void AddMusic()
        {
			var path = _fileService.SelectFile("Add Music", new string[] {".mp3"});

			if (path != null)
			{
                var destination = _fileService.SaveFileForUser<RadioChannelEntity>(path, _userStore.CurrentUser);
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
            if (_userStore.IsLoggedIn)
            {
			    _radioChannel.dbEntities.AddRange(_fileService.GetUserRadioFiles(_userStore.CurrentUser));
            }
            
            await _radioChannel.GetDefaultItemsAsync();

            MusicChannels = new ObservableCollection<RadioChannelEntity>(_radioChannel.dbEntities);
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
