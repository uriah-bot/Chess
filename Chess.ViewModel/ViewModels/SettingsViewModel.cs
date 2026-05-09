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
		private readonly IFileService _fileService;
		private readonly ICustomizableDecorManager<BoardThemeEntity> _boardThemeManager;
		private readonly ICustomizableDecorManager<PieceThemeEntity> _pieceThemeManager;
		private readonly ICustomizableDecorManager<RadioChannelEntity> _radioChannel;

        public SettingsViewModel(IUserStore userStore, INavigationService navigationService, IWindowService windowService, IUserRepository userRepository, IDecorStore decorStore, IFileService fileService,
			ICustomizableDecorManager<BoardThemeEntity> boardThemeManager, ICustomizableDecorManager<PieceThemeEntity> pieceThemeManager, ICustomizableDecorManager<RadioChannelEntity> radioChannel)
        {
            _userStore = userStore;
			_navigationService = navigationService;
			_windowService = windowService;
			_userRepo = userRepository;
			_boardThemeManager = boardThemeManager;
			_pieceThemeManager = pieceThemeManager;
			_radioChannel = radioChannel;
			_decorStore = decorStore;
			_fileService = fileService;

            _userStore.CurrentUserChanged += OnUserChanged;

			DeleteUserCommand = new RelayCommand(o => DeleteUser(), o => CanDeleteUser());
			PlaySelectedMusicCommand = new RelayCommand(o => PlayMusic(o));
            ShowPopupCommand = new RelayCommand(o => ShowPopup(o));
			AddMusicCommand = new RelayCommand(o => AddMusic());
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
            if (_userStore.CurrentUser == null) return;

            var property = o as string;
            _userStore.AppendingPropertyChange = property;
            _windowService.ShowDialog<AccountModificationMenuViewModel>();
		}

        private async Task LoadChannelsAsync()
        {
			await _radioChannel.GetDefaultItemsAsync(_userStore.CurrentUser);
			_radioChannel.dbEntities.AddRange(_fileService.GetUserRadioFiles(_userStore.CurrentUser));

            MusicChannels = new ObservableCollection<RadioChannelEntity>(_radioChannel.dbEntities);
        }

        private void PlayMusic(object content)
        {
			string musicName = content as string;
			if (musicName != null)
			{
				var music = MusicChannels.FirstOrDefault(m => m.ChannelName == musicName);
				if (music != null)
				{
                    _decorStore.CurrentSong = new Uri(Path.Combine(AppConstants.BASE_PATH, music.ChannelPath), UriKind.RelativeOrAbsolute);
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

        private async void DeleteUser()
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
