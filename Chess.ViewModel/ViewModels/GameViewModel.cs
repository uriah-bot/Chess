using Chess.Model;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Chess.ViewModel
{
    public partial class GameViewModel : ViewModelBase
    {
		private readonly IUserStore _userStore;
		private readonly IGameManagerService _gameManager;
		private readonly IWindowService _windowService;

        public ObservableCollection<SquareViewModel> Squares { get; } = new ObservableCollection<SquareViewModel>();
        private List<Move> _availableMoves = new List<Move>();
        private List<Position> _markedSquares = new List<Position>();
        private Position _selectedPosition = null;

        public GameViewModel(IUserStore userStore, IGameManagerService gameManagerService, IWindowService windowService)
		{
			_userStore = userStore;
			_gameManager = gameManagerService;
			_windowService = windowService;

            InitializeBoard();
            RestartGame();

            _gameManager.Game.OnGameEndedByTimer += EndGame;
        }

        private void EndGame()
        {
            // A background timer might have ended the game, so we use Dispatcher
            Application.Current.Dispatcher.Invoke(async () =>
            {
                _gameManager.Game.OnGameEndedByTimer -= EndGame;

                await _gameManager.EndGameAsync(_userStore.CurrentUser);

                _windowService.ShowDialog<GameOverMenuViewModel>();
                
            });
        }

        private void InitializeBoard()
        {
            Squares.Clear();
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // Pass the OnSquareClicked method to every square
                    Squares.Add(new SquareViewModel(row, col, OnSquareLeftClicked, OnSquareRightClicked));
                }
            }
        }

        private void OnSquareRightClicked(Position position)
        {
            if (!(_markedSquares.RemoveAll(p => p == position) > 0))
            {
                _markedSquares.Add(position);
            }

            UpdateBoardVisuals();
        }

        private void OnSquareLeftClicked(Position pos)
        {
            if (_selectedPosition == null)
            {
                SelectPiece(pos);
            }
            else
            {
                TryMakeMove(pos);
            }
        }

        private void SelectPiece(Position pos)
        {
            _markedSquares.Clear();

            IEnumerable<Move> moves = _gameManager.Game.LegalMovesForPiece(pos);
            if (moves.Any())
            {
                _selectedPosition = pos;
                _availableMoves = moves.ToList();
            }
            else
            {
                _selectedPosition = null;
                _availableMoves.Clear();
            }

            UpdateBoardVisuals(); // Refreshes images and highlights
        }

        private void TryMakeMove(Position targetPos)
        {
            var possibleMoves = _availableMoves.Where(m => m.ToPosition == targetPos).ToList();

            if (possibleMoves.Count == 0)
            {
                SelectPiece(targetPos);
                return;
            }

            if (possibleMoves.Count > 1 && possibleMoves.First().Type == MoveType.Promotion)
            {
                _gameManager.PendingPromotionMoves = possibleMoves;
                _windowService.ShowDialog<PromotionMenuViewModel>();

                _availableMoves.Clear();
                _selectedPosition = null;

                UpdateBoardVisuals();
            }
            else
            {
                _gameManager.Game.MakeMove(possibleMoves.First());
                _selectedPosition = null;
                _availableMoves.Clear();

                UpdateBoardVisuals();

                if (_gameManager.Game.IsGameOver())
                {
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        EndGame();
                    }, System.Windows.Threading.DispatcherPriority.ContextIdle);
                }
            }
        }

        private void UpdateBoardVisuals()
        {
            foreach (var square in Squares)
            {
                square.Piece = _gameManager.Game.Board[square.Position.Row, square.Position.Column];

                if (_markedSquares.Contains(square.Position))
                {
                    square.OverlayBrush = new SolidColorBrush(Color.FromArgb(130, 246, 31, 31)); // red
                }
                else
                {
                    square.OverlayBrush = Brushes.Transparent;
                }

                if (_selectedPosition == square.Position)
                {
                    square.HighlightBrush = new SolidColorBrush(Color.FromArgb(150, 170, 94, 220)); // Selected // purple
                }
                else if (_availableMoves.Where(m => m.ToPosition == square.Position).Any())
                {
                    square.HighlightBrush = new SolidColorBrush(Color.FromArgb(150, 125, 255, 125)); // Legal Move // green
                }
                else
                {
                    square.HighlightBrush = Brushes.Transparent;
                }
            }

            OnPropertyChanged(nameof(CurrentPlayer));
        }

        // properties for binding to the UI
        public string Username => _userStore.CurrentUser?.Username ?? "Stranger";
		public string AIName => _gameManager.BotRating?.ToString() ?? string.Empty;
		public bool IsClassicalGame => _gameManager.Mode == GameMode.Classical;
        public PlayerColor CurrentPlayer => _gameManager.Game.CurrentPlayer;

        private string _whiteUserTimerText;
		public string WhiteUserTimerText
		{
			get
			{
				return _whiteUserTimerText;
			}
			set
			{
				_whiteUserTimerText = value;
				OnPropertyChanged(nameof(WhiteUserTimerText));
			}
		}

        private string _blackUserTimerText;
		public string BlackUserTimerText
		{
			get
			{
				return _blackUserTimerText;
			}
			set
			{
				_blackUserTimerText = value;
				OnPropertyChanged(nameof(BlackUserTimerText));
			}
		}

		private void TimersUpdated(string key, string value)
		{
			Application.Current.Dispatcher.BeginInvoke(() =>
			{
				if (key == "WhiteTime")
				{
					WhiteUserTimerText = value;
				}
				else if (key == "BlackTime")
				{
					BlackUserTimerText = value;
				}
			});
        }

		public void RestartGame()
		{
            _markedSquares.Clear();
            _gameManager.ConfigurateGame();

            if (_gameManager.Modifiers.Any(m => m.Modifier == ModifierType.TimeLimit))
            {
                _gameManager.Game.OnModifierDataUpdated += TimersUpdated;

                var modifierStartTime = TimeSpan.FromMinutes(int.Parse(_gameManager.Modifiers.FirstOrDefault(m => m.Modifier == ModifierType.TimeLimit)?.SelectedParameter)).ToString(@"mm\:ss");
                WhiteUserTimerText = modifierStartTime;
                BlackUserTimerText = modifierStartTime;
            }

            UpdateBoardVisuals();
        }


        public override void Dispose()
        {
            if (_gameManager.Modifiers.Any(m => m.Modifier == ModifierType.TimeLimit))
            {
                _gameManager.Game.OnModifierDataUpdated -= TimersUpdated;
            }

            base.Dispose();
        }
	}
}
