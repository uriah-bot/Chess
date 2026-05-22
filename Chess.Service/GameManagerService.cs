using Chess.Model;
using Chess.Service;

namespace Chess.ViewModel.ViewModelHelper
{
    public interface IGameManagerService
    {
        Game Game { get; set; }
        GameMode Mode { get; set; }
        List<Move> PendingPromotionMoves { get; set; }
        PlayerColor UserColor { get; set; }
        int? BotRating { get; set; }
        List<ActiveModifier> Modifiers { get; set; }
        bool IsBoardReactive { get; }
        void ConfigurateGame();
        Task EndGameAsync(UserEntity currentUser);
        void MoveHuman(Move move);
        Task MoveStockfishAsync();
    }

    public class GameManagerService : IGameManagerService
    {
        private readonly Random rnd = new Random();
        private readonly StockfishCommunicationService _stockfishCommunicationService;
        private readonly IGameService _gameService;

        public Game Game { get; set; }
        public GameMode Mode { get; set; } = GameMode.Classical;
        public PlayerColor UserColor { get; set; } = PlayerColor.White;
        public bool IsBoardReactive { get; private set; }
        public List<Move> PendingPromotionMoves { get; set; } = new List<Move>();
        public List<ActiveModifier> Modifiers { get; set; } = new List<ActiveModifier>();
        public int? BotRating { get; set; }
        public Queue<string> Moves { get; set; } = new Queue<string>();
        private DateTime Time { get; set; }
        private int? EloDelta { get; set; }

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
            EloDelta = CalculateEloChange(Game);

            GameEntity currentGame = new GameEntity
            {
                UserId = user.Id,
                UserPlayedAs = Mode == GameMode.Classical ? UserColor : null,
                BotRating = Mode == GameMode.Classical ? BotRating : null,
                Modifiers = Modifiers.Select(m => m.Modifier).ToList(),
                Result = Mode == GameMode.Modified ? null : Game.Result.winner switch
                {
                    var winner when winner == UserColor => "Win",
                    PlayerColor.None => "Draw",
                    _ => "Loss"
                },
                EloDelta = EloDelta,
                DatePlayed = Time
            };

            //while (Moves.Count > 0)
            //{
            //    currentGame.GameMoves.Add(Moves.Dequeue());
            //}

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

        public void MoveHuman(Move move)
        {
            Game.MakeMove(move);
            Moves.Enqueue(MoveFormatter.MoveToString(move));
            IsBoardReactive = Mode == GameMode.Classical ? !IsBoardReactive : IsBoardReactive;
        }

        public async Task MoveStockfishAsync()
        {
            var fen = new FEN(Game.Board, Game.CurrentPlayer);

            string bestMove = await _stockfishCommunicationService.GetBotMoveAsync(fen, BotRating);

            var move = MoveFormatter.ParseStockfishMove(Game.Board, bestMove);
            Game.MakeMove(move);
            Moves.Enqueue(MoveFormatter.MoveToString(move));
            IsBoardReactive = Mode == GameMode.Classical ? !IsBoardReactive : IsBoardReactive;
        }


        private void Cleanup()
        {
            Game.EndMatch();
            _stockfishCommunicationService?.Dispose();
        }
    }
}