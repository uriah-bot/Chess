using Chess.Model;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Chess.ViewModel
{
    public class GamePausedMenuViewModel : ViewModelBase, IDialogViewModel
    {
        public Action RequestClose { get; set; }
        public IGameManagerService _gameManagerService;
        public INavigationService _navigationService;
        public IUserStore _userStore;
        public IWindowService _windowService;

        public ICommand ContinueCommand { get; }
        public ICommand ResignCommand { get; }
        public ICommand PlayAgainCommand { get; }
        public ICommand ExitCommand { get; }
        public string Mode { get; }

        public GamePausedMenuViewModel(IGameManagerService gameManagerService, INavigationService navigationService, IUserStore userStore, IWindowService windowService)
        {
            _gameManagerService = gameManagerService;
            _navigationService = navigationService;
            _userStore = userStore;
            _windowService = windowService;

            Mode = _gameManagerService.Mode == GameMode.Classical ? "Classical" : "Modified";

            ContinueCommand = new RelayCommand(o => RequestClose?.Invoke());
            ExitCommand = new RelayCommand(o => { RequestClose?.Invoke(); _windowService.SwitchWindow<AppBaseViewModel>(); });
            PlayAgainCommand = new RelayCommand(async o => await PlayAgain());
            ResignCommand = new RelayCommand(async o =>
            {
                _gameManagerService.Game.Resign(_gameManagerService.UserColor);
                await _gameManagerService.EndGameAsync(_userStore.CurrentUser);
                RequestClose?.Invoke();
            });
        }

        private async Task PlayAgain()
        {
            RequestClose?.Invoke();

            await _gameManagerService.EndGameAsync(_userStore.CurrentUser);
            _navigationService.NavigateTo<GameViewModel>();
        }
    }
}
