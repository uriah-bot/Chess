using Chess.Model;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class ClassicalViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;
        private readonly IGameConfigurationStore _gameConfigStore;
        public ICommand NavigateToSettingsCommand { get; }
        public ICommand StartClassicalGameCommand { get; }

        public ClassicalViewModel(INavigationService navigationService, IWindowService windowService, IGameConfigurationStore gameConfigurationStore)
        {
            _navigationService = navigationService;
            _windowService = windowService;
            _gameConfigStore = gameConfigurationStore;
            NavigateToSettingsCommand = new NavigateCommand<SettingsViewModel>(_navigationService);
            StartClassicalGameCommand = new RelayCommand(o => ExecuteStartClassicalGame());
        }

        public PlayerColor UserColor => PlayerColor.None; // TODO: change this to user selected color

        public void ExecuteStartClassicalGame()
        {
            _gameConfigStore.Mode = GameMode.Classical;
            _gameConfigStore.UserColor = UserColor;

            _windowService.SwitchWindow<MainViewModel>();
        }
    }
}
