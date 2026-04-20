using Chess.Model;
using Chess.ViewModel.Stores;
using System.Collections.ObjectModel;

namespace Chess.ViewModel
{
    public class StatsViewModel : ViewModelBase
    {
        private readonly IUserStore _userStore;
        private readonly IGameHistoryStore _gameHistoryStore;

        public ObservableCollection<GameRecord> GameHistory { get; } = new ObservableCollection<GameRecord>();

        public StatsViewModel(IUserStore userStore, IGameHistoryStore gameHistoryStore)
        {
            _userStore = userStore;
            _gameHistoryStore = gameHistoryStore;
            _userStore.CurrentUserChanged += OnUserChanged;

            _ = ReloadDataAsync();
            
        }

        private async Task ReloadDataAsync()
        {
            GameHistory.Clear();

            await _gameHistoryStore.LoadGamesAsync();

            if (_userStore.CurrentUser == null)
            {
                return;
            }

            foreach (var game in _gameHistoryStore.UserGames)
            {
                GameHistory.Add(new GameRecord(game));
            }
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

    public readonly record struct GameRecord
    {
        public GameRecord(GameEntity game)
        {
            GameMode = game.BotRating == null ? "Player vs Player" : "Player vs AI";
            AIName = game.BotRating == null ? string.Empty :"Stockfish" + game.BotRating.ToString();
            UserColor = game.BotRating == null ? string.Empty : game.UserPlayedAs.ToString();
            Result = game.BotRating == null ? "(friendly game)" : game.Result.ToString();
            Date = game.DatePlayed.ToString("yy-MM-dd--hh--mm");
        }

        public string GameMode { get; }
        public string AIName { get; }
        public string Result { get; }
        public string Date { get; }
        public string UserColor { get; }
        public string ResultColor {
            get
            {
                return Result switch
                {
                    "Win" => "Green",
                    "Loss" => "Red",
                    "Draw" => "Yellow",
                    _ => "Black"
                };
            }
        }
    }
}
