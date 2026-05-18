using Chess.Model;
using Chess.Service;

namespace Chess.ViewModel.ViewModelHelper
{
    public interface IGameManagerService
    {
        Game Game { get; set; }
        GameMode Mode { get; set; }
        Move LastMove { get; set; }
        List<Move> PendingPromotionMoves { get; set; }
        PlayerColor UserColor { get; set; }
        int? BotRating { get; set; }
        List<ActiveModifier> Modifiers { get; set; }
        DateTime Time { get; set; }
        bool IsBoardReactive { get; set; }
        void ConfigurateGame();
        Task EndGameAsync(UserEntity currentUser);
        void HumanMoveAsync(Move move);
        Task StockfishMoveAsync();
    }

    public class GameManagerService : IGameManagerService
    {
        private readonly Random rnd = new Random();
        private readonly StockfishCommunicationService _stockfishCommunicationService;
        private readonly IGameService _gameService;

        public GameMode Mode { get; set; } = GameMode.Classical;
        public PlayerColor UserColor { get; set; } = PlayerColor.White;
        public bool IsBoardReactive { get; set; }
        public int? BotRating { get; set; }
        public DateTime Time { get; set; }
        public Move LastMove { get; set; }
        public List<Move> PendingPromotionMoves { get; set; } = new List<Move>();
        public Queue<string> Moves { get; set; } = new Queue<string>();
        public List<ActiveModifier> Modifiers { get; set; } = new List<ActiveModifier>();
        public Game Game { get; set; }

        public GameManagerService(IGameService gameService, StockfishCommunicationService stockfishCommunicationService)
        {
            _gameService = gameService;
            _stockfishCommunicationService = stockfishCommunicationService;
        }

        public void ConfigurateGame()
        {
            Game game = new Game(PlayerColor.White, Board.Initial());
            Game = game;
            Time = DateTime.Now;
            IsBoardReactive = Mode != GameMode.Classical || UserColor == PlayerColor.White;

            if (Mode == GameMode.Classical)
            {
                _stockfishCommunicationService.StartEngine(AppConstants.STOCKFISH_PATH_TO_EXE);
            }
            
            Game.StartMatch(Modifiers);
        }

        public async Task EndGameAsync(UserEntity user)
        {
            Cleanup();

            GameEntity currentGame = new GameEntity
            {
                UserId = user.Id,
                UserPlayedAs = Mode == GameMode.Classical ? UserColor : null,
                BotRating = Mode == GameMode.Classical ? BotRating : null,
                Modifiers = Modifiers.Select(m => m.Modifier).ToList(),
                Result = Game.Result.winner switch
                {
                    var winner when winner == UserColor => "Win",
                    PlayerColor.None => "Draw",
                    _ => "Loss"
                },
                EloDelta = CalculateEloChange(Game),
                DatePlayed = Time
            };

            while (Moves.Count > 0)
            {
                currentGame.GameMoves.Add(Moves.Dequeue());
            }

            await _gameService.SaveGameAsync(currentGame); 
        }

        private int? CalculateEloChange(Game game)
        {
            if (Mode != GameMode.Classical || BotRating == null)
                return null;

            int C = rnd.Next(5, 15);

            var score = BotRating.Value / 100;
            var multi = game.Result.winner switch
            {
                PlayerColor.None => 0,
                var winner when winner == UserColor && BotRating >= 1800 => 0.6,
                var winner when winner == UserColor => 0.8,
                _ => -0.9
            };

            return (int)Math.Round(multi * (score + C));
        }

        public void HumanMoveAsync(Move move)
        {
            if (!IsBoardReactive)
                return;

            Game.MakeMove(move);
            Moves.Enqueue(MoveFormatter.MoveToString(move));
            LastMove = Game.Result == null ? move : null;
        }

        public async Task StockfishMoveAsync()
        {
            if (IsBoardReactive)
                return;

            var fen = new FEN(Game.Board, Game.CurrentPlayer);

            string bestMove = await _stockfishCommunicationService.GetBotMoveAsync(fen, BotRating);

            var move = MoveFormatter.ParseStockfishMove(Game.Board, bestMove);
            Game.MakeMove(move);
            Moves.Enqueue(MoveFormatter.MoveToString(move));
            LastMove = move;
        }

        public void Cleanup()
        {
            Game.EndMatch();
            _stockfishCommunicationService?.Dispose();
        }
    }
}