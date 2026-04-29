using Chess.Model;
using Chess.Service;
using Chess.ViewModel.Stores;

namespace Chess.ViewModel.ViewModelHelper
{
    public interface IGameManagerService
    {
        GameMode Mode { get; set; }
        PlayerColor UserColor { get; set; }
        int? BotRating { get; set; }
        DateTime Time { get; set; }
        PlayerColor CurrentTurn { get; set; }
        bool IsBoardReactive => CurrentTurn == UserColor;
        Move LastMove { get; set; }
        Game ConfigurateGame(List<ModifierType> modifiers);
        Task EndGameAsync(Game game);
    }

    public class GameManagerService : IGameManagerService
    {
        private readonly IGameHistoryStore _gameHistoryStore;
        private readonly IUserStore _userStore;
        private readonly StockfishCommunicationService _stockfishCommunicationService;
        private readonly StockfishHelper _stockfishHelper;

        public GameMode Mode { get; set; } = GameMode.Classical;
        public PlayerColor UserColor { get; set; } = PlayerColor.White;
        public int? BotRating { get; set; }
        public DateTime Time { get; set; }
        public PlayerColor CurrentTurn { get; set; }
        public List<ModifierType> Modifiers { get; set; } = new List<ModifierType>();
        public Move LastMove { get; set; }

        public GameManagerService(IGameHistoryStore gameHistoryStore, IUserStore userStore, StockfishCommunicationService stockfishCommunicationService, StockfishHelper stockfishHelper)
        {
            _gameHistoryStore = gameHistoryStore;
            _userStore = userStore;
            _stockfishCommunicationService = stockfishCommunicationService;
            _stockfishHelper = stockfishHelper;
        }

        public Game ConfigurateGame(List<ModifierType> modifiers = null)
        {
            Game game = new Game(PlayerColor.White, Board.Initial());
            game.Mode = Mode;
            Time = DateTime.Now;
            Modifiers = modifiers ?? new List<ModifierType>();

            return game;
        }

        public async Task EndGameAsync(Game game)
        {
            GameEntity currentGame = new GameEntity
            {
                Username = _userStore.CurrentUser.Username,
                GameMode = game.Mode,
                UserPlayedAs = game.Mode == GameMode.Classical ? UserColor : null,
                BotRating = game.Mode == GameMode.Classical ? BotRating : null,
                //Result = null,
                DatePlayed = Time
            };

            await _gameHistoryStore.SaveGameAsync(currentGame);

            Cleanup();
        }

        public void Cleanup()
        {
            _stockfishCommunicationService?.Dispose();
        }
    }
}