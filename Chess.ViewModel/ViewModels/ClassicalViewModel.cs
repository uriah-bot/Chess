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
        public ICommand NavigateToSettingsCommand { get; }
        public ICommand StartClassicalGameCommand { get; }

        public ClassicalViewModel(INavigationService navigationService, IWindowService windowService, IGameManagerService gameManagerService)
        {
            _navigationService = navigationService;
            _windowService = windowService;
            _gameManagerService = gameManagerService;
            NavigateToSettingsCommand = new NavigateCommand<SettingsViewModel>(_navigationService);
            StartClassicalGameCommand = new RelayCommand(o => ExecuteStartClassicalGame());
        }

        public PlayerColor UserColor => PlayerColor.None; // TODO: change this to user selected color

        public void ExecuteStartClassicalGame()
        {
            _gameManagerService.Mode = GameMode.Classical;
            _gameManagerService.UserColor = UserColor;

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
