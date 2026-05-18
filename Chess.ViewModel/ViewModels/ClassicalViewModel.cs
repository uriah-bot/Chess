using Chess.Model;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class ClassicalViewModel : ViewModelBase
    {
        private static readonly Random rnd = new Random();
        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;
        private readonly IGameManagerService _gameManagerService;
        private readonly IUserStore _userStore;

        public ClassicalViewModel(INavigationService navigationService, IWindowService windowService, IGameManagerService gameManagerService, IUserStore userStore)
        {
            _navigationService = navigationService;
            _windowService = windowService;
            _gameManagerService = gameManagerService;
            _userStore = userStore;

            NavigateToSettingsCommand = new NavigateCommand<SettingsViewModel>(_navigationService);
            StartClassicalGameCommand = new RelayCommand(o => StartClassicalGame(), o => _userStore.IsLoggedIn);
            SelectColorCommand = new RelayCommand(o => SelectColor(o));
            SelectBotCommand = new RelayCommand(o => SelectBot(o));
        }

        public ICommand NavigateToSettingsCommand { get; }
        public ICommand StartClassicalGameCommand { get; }
        public ICommand SelectColorCommand { get; }
        public ICommand SelectBotCommand { get; }

        public PlayerColor UserColor { get; set; } = PlayerColor.White;
        public int SelectedBotElo { get; set; } = 1200;

        private void SelectColor(object o)
        {
            var color = o as string;

            UserColor = color switch
            {
                "White" => PlayerColor.White,
                "Black" => PlayerColor.Black,
                _ => RandomizePlayerColor(),
            };
        }

        private void SelectBot(object o)
        {
            var botElo = o as int?;

            SelectedBotElo = botElo ?? -1;
        }

        public void StartClassicalGame()
        {
            _gameManagerService.UserColor = UserColor;
            _gameManagerService.BotRating = SelectedBotElo;
            _gameManagerService.Mode = GameMode.Classical;

            _navigationService.NavigateTo<GameViewModel>();
            _windowService.SwitchWindow<MainViewModel>();
        }

        private PlayerColor RandomizePlayerColor()
        {
            int random = rnd.Next(2);

            return random switch
            {
                0 => PlayerColor.White,
                1 => PlayerColor.Black,
                _ => PlayerColor.None // if somehow you managed to (just quit at that point)
            };
        }
    }
}
