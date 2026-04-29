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
		private readonly ICustomizableDecorManager<BoardThemeEntity> _boardThemeManager;
		private readonly ICustomizableDecorManager<PieceThemeEntity> _pieceThemeManager;
		private readonly ICustomizableDecorManager<RadioChannelEntity> _radioChannel;

        public SettingsViewModel(IUserStore userStore, INavigationService navigationService, IWindowService windowService, IUserRepository userRepository, IDecorStore decorStore,
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

            _username = _userStore.CurrentUser?.Username ?? "Stranger";

			DeleteUserCommand = new RelayCommand(o => DeleteUser(), o => CanDeleteUser());
			PlaySelectedMusicCommand = new RelayCommand(o => PlayMusic(o));
			ShowPopupCommand = new RelayCommand(o => ShowPopup(o));
            _ = LoadChannelsAsync();
        }

        private void ShowPopup(object parameter)
        {
			var property = parameter as string;

			switch (property)
			{
				case "Role":
                    _windowService.ShowDialog<GamePausedMenuViewModel>();
                    break;
				case "Username":
                    _windowService.ShowDialog<GamePausedMenuViewModel>();
                    break;
				case "Password":
					_windowService.ShowDialog<GamePausedMenuViewModel>();
					break;
                default:
					break;
            }
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

        private async Task LoadChannelsAsync()
        {
			await _radioChannel.GetItemsForUserAsync(_userStore.CurrentUser);

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

        public ICommand DeleteUserCommand { get; }
		public ICommand PlaySelectedMusicCommand { get; }
		public ICommand ShowPopupCommand { get; }
		public ICommand RequestRoleCommand { get; }
        public ICommand AddMusicCommand { get; }
        
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

        private string _username;
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
                _username = value;
				OnPropertyChanged(nameof(Username));
			}
		}

		public string Role => _userStore.CurrentUser?.Role.ToString() ?? "Guest";

		public int Volume
		{
			get => (int)(_decorStore.CurrentVolume * 100);
			set
			{
				_decorStore.CurrentVolume = (double)value /100;
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
	}
}
