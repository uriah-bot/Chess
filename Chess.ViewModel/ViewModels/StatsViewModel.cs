using Chess.Model;
using Chess.Service;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Collections.ObjectModel;

namespace Chess.ViewModel
{
    public class StatsViewModel : ViewModelBase
    {
        // TODO: ADD A STORE FOR GAME HISTORY IN MEMORY
        private readonly IUserStore _userStore;
        private readonly IGameService _gameService;
        private readonly IWindowService _windowService;
        private readonly INavigationService _navigationService;
        private readonly IGameReplayRequestStore _replayRequestStore;

        public ObservableCollection<GameEntityViewModel> GameHistory { get; } = new ObservableCollection<GameEntityViewModel>();

        public StatsViewModel(IUserStore userStore, IGameService gameService, IWindowService windowService, INavigationService navigationService, IGameReplayRequestStore replayRequestStore)
        {
            _userStore = userStore;
            _gameService = gameService;
            _windowService = windowService;
            _navigationService = navigationService;
            _replayRequestStore = replayRequestStore;

            _userStore.CurrentUserChanged += OnUserChanged;

            _ = ReloadDataAsync();
        }

        private async Task ReloadDataAsync()
        {
            GameHistory.Clear();

            if (_userStore.CurrentUser == null)
            {
                return;
            }

            var games = await _gameService.GetGamesByUserAsync(_userStore.CurrentUser ?? new UserEntity());

            if (games == null || games.Count == 0)
            {
                return;
            }

            foreach (var game in games)
            {
                GameHistory.Add(new GameEntityViewModel(game, () => OpenReplayWindow(game)));
            }
        }

        private void OpenReplayWindow(GameEntity game)
        {
            _replayRequestStore.RequestedGame = game;
            _navigationService.NavigateTo<GameViewModel>();
            _windowService.SwitchWindow<MainViewModel>();
        }

        public int Wins => _userStore.CurrentUser?.Wins ?? -1;
        public int Draws => _userStore.CurrentUser?.Draws ?? -1;
        public int Losses => _userStore.CurrentUser?.Losses ?? -1;

        public int TotalMatches => Wins == -1 ? -1 : Wins + Draws + Losses;

        public double WinRate
        {
            get
            {
                if (Wins == -1)
                {
                    return -1;
                }
                else if (TotalMatches == 0)
                {
                    return 0;
                }
                
                return Math.Round((double) 100 * Wins / TotalMatches, 3);
            }
        }
		public int WinRateBar => (int) (WinRate);
		public int Elo => _userStore.CurrentUser?.Elo ?? -1;
		public int PeakElo => _userStore.CurrentUser?.PeakElo ?? -1;

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(Wins));
            OnPropertyChanged(nameof(Draws));
            OnPropertyChanged(nameof(Losses));
            OnPropertyChanged(nameof(TotalMatches));
            OnPropertyChanged(nameof(WinRate));
            OnPropertyChanged(nameof(WinRateBar));
            OnPropertyChanged(nameof(Elo));
            OnPropertyChanged(nameof(PeakElo));
            OnPropertyChanged(nameof(GameHistory));
        }

        public override void Dispose()
        {
            _userStore.CurrentUserChanged -= OnUserChanged;
            base.Dispose();
        }
    }
}
