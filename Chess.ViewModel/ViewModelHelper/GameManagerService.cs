using Chess.Model;
using Chess.Service;
using Chess.ViewModel.Stores;

namespace Chess.ViewModel.ViewModelHelper
{
    public interface IGameManagerService
    {
        Game Game { get; set; }
        Move LastMove { get; set; }
        PlayerColor UserColor { get; set; }
        int? BotRating { get; set; }
        DateTime Time { get; set; }
        bool IsBoardReactive { get; set; }
        Game ConfigurateGame(List<ModifierType> modifiers, int? botRating = null);
        Task EndGameAsync(Game game);
        void HumanMoveAsync(Move move);
        Task StockfishMoveAsync();
    }

    public class GameManagerService : IGameManagerService
    {
        private readonly IGameHistoryStore _gameHistoryStore;
        private readonly IUserStore _userStore;
        private readonly StockfishCommunicationService _stockfishCommunicationService;
        private readonly GameLogicHelper _stockfishHelper;

        public GameMode Mode { get; set; } = GameMode.Classical;
        public PlayerColor UserColor { get; set; } = PlayerColor.White;
        public bool IsBoardReactive { get; set; }
        public int? BotRating { get; set; }
        public DateTime Time { get; set; }
        public Move LastMove { get; set; }
        public List<ModifierType> Modifiers { get; set; } = new List<ModifierType>();
        public Game Game { get; set; }

        public GameManagerService(IGameHistoryStore gameHistoryStore, IUserStore userStore, StockfishCommunicationService stockfishCommunicationService, GameLogicHelper stockfishHelper)
        {
            _gameHistoryStore = gameHistoryStore;
            _userStore = userStore;
            _stockfishCommunicationService = stockfishCommunicationService;
            _stockfishHelper = stockfishHelper;
        }

        public Game ConfigurateGame(List<ModifierType> modifiers = null, int? botRating = null)
        {
            Game game = new Game(PlayerColor.White, Board.Initial());
            Game = game;
            Mode = modifiers != null ? GameMode.Modified : GameMode.Classical;
            Time = DateTime.Now;
            Modifiers = modifiers ?? new List<ModifierType>();
            BotRating = botRating;
            IsBoardReactive = Mode != GameMode.Classical || UserColor == PlayerColor.White;

            _stockfishCommunicationService.StartEngine(AppConstants.STOCKFISH_PATH_TO_EXE);
            Game.StartMatch(Modifiers);

            return game;
        }

        public async Task EndGameAsync(Game game)
        {
            GameEntity currentGame = new GameEntity
            {
                Username = _userStore.CurrentUser.Username,
                UserPlayedAs = Mode == GameMode.Classical ? UserColor : null,
                BotRating = Mode == GameMode.Classical ? BotRating : null,
                Result = game.Result.winner switch
                {
                    var winner when winner == UserColor => "Win",
                    PlayerColor.None => "Draw",
                    _ => "Loss"
                },
                DatePlayed = Time
            };

            await _gameHistoryStore.SaveGameAsync(currentGame);

            Cleanup();
        }

        public void HumanMoveAsync(Move move)
        {
            if (!IsBoardReactive)
                return;

            Game.MakeMove(move);
            LastMove = Game.Result == null ? move : null;
        }

        public async Task StockfishMoveAsync()
        {
            if (IsBoardReactive)
                return;

            var fen = new FEN(Game.Board, Game.CurrentPlayer);

            string bestMove = await _stockfishCommunicationService.GetBotMoveAsync(fen, BotRating);

            var move = _stockfishHelper.ParseMove(Game.Board, bestMove);
            Game.MakeMove(move);
            LastMove = Game.Result == null ? move : null;
        }

        public void Cleanup()
        {
            _stockfishCommunicationService?.Dispose();
        }
    }
}