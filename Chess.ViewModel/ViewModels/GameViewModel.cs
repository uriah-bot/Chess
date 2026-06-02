using Chess.Model;
using Chess.Service;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess.ViewModel
{
    public partial class GameViewModel : ViewModelBase
    {
		private readonly IUserStore _userStore;
		private readonly IGameManagerService _gameManager;
		private readonly IWindowService _windowService;
		private readonly IGameReplayRequestStore _gameReplayStore;

        public ObservableCollection<SquareViewModel> Squares { get; } = new ObservableCollection<SquareViewModel>();
        private List<Move> _availableMoves = new List<Move>();
        private List<Position> _markedSquares = new List<Position>();
        private Position _selectedPosition = null;

        private List<Board> _boardPositions { get; } = new List<Board>();
        private int _currentBoard;
        public bool IsReplayMode => _gameReplayStore.IsReplayRequested;
        private Board CurrentRenderedBoard => IsReplayMode ? _boardPositions[_currentBoard] : _gameManager.Game.Board;

        public GameViewModel(IUserStore userStore, IGameManagerService gameManagerService, IWindowService windowService, IGameReplayRequestStore gameReplayStore)
		{
			_userStore = userStore;
			_gameManager = gameManagerService;
			_windowService = windowService;
			_gameReplayStore = gameReplayStore;

            // basically changing (because of mode) the usercolor or maintaining if restarted the game
            _gameManager.UserColor = _gameManager.Mode != GameMode.Classical ? PlayerColor.White : _gameManager.UserColor;

            InitializeBoard();
            RestartGame();

            if (IsReplayMode)
                return;

            PauseGameCommand = new RelayCommand(o => _windowService.ShowDialog<GamePausedMenuViewModel>());

            _gameManager.Game.OnGameEndedByTimer += EndGame;
        }

        public ICommand ReplayForwardsCommand { get; private set; }
        public ICommand ReplayBackwardsCommand { get; private set; }
        public ICommand StopReplayCommand { get; private set; }
        public ICommand PauseGameCommand { get; }

        private void EndGame()
        {
            // A background timer might have ended the game, so we use Dispatcher
            Application.Current?.Dispatcher.InvokeAsync(async () =>
            {
                _gameManager.Game.OnGameEndedByTimer -= EndGame;


                _userStore.UpdateOnGameEnd(_gameManager);

                await _gameManager.EndGameAsync(_userStore.CurrentUser);

                _windowService.ShowDialog<GameOverMenuViewModel>();

            }, System.Windows.Threading.DispatcherPriority.ContextIdle); // do it when all else is done in the thread;
        }

        private void InitializeBoard()
        {
            Squares.Clear();

            var isPlayingAsBlack = _gameManager.Mode == GameMode.Classical && _gameManager.UserColor == PlayerColor.Black;

            int start = isPlayingAsBlack ? 7 : 0;
            int end = isPlayingAsBlack ? -1 : 8;
            int step = isPlayingAsBlack ? -1 : 1;

            for (int row = start; row != end; row += step)
            {
                for (int col = start; col != end; col += step)
                {
                    string coordinate = string.Empty;

                    if (row == 7 - start || col == start)
                    {
                        coordinate = $"{(char)('a' + col)}{8 - row}".ToUpper();
                    }

                    if (!_userStore.CurrentUser.Settings.DisplayCoordinates)
                        coordinate = string.Empty;

                    // pass OnSquareClicked method to every square
                    Squares.Add(new SquareViewModel(row, col, coordinate, OnSquareLeftClicked, OnSquareRightClicked));
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

        private async void OnSquareLeftClicked(Position pos)
        {
            if (_gameManager.Mode == GameMode.Classical && !_gameManager.IsBoardReactive || IsReplayMode) return;

            if (_selectedPosition == null)
            {
                SelectPiece(pos);
            }
            else
            {
                await TryMakeMove(pos);
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

        private async Task TryMakeMove(Position targetPos)
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

                _selectedPosition = null;
                _availableMoves.Clear();

                UpdateBoardVisuals();

                if (_gameManager.Game.IsGameOver())
                {
                    EndGame();
                    return;
                }

                await TriggerBotAsync();

                return;
            }

            _gameManager.MoveHuman(possibleMoves.First());
            _selectedPosition = null;
            _availableMoves.Clear();

            UpdateBoardVisuals();

            if (_gameManager.Game.IsGameOver())
            {
                EndGame();
                return;
            }

            await TriggerBotAsync();
        }

        private async Task TriggerBotAsync()
        {
            if (_gameManager.Mode == GameMode.Modified || _gameManager.IsBoardReactive) return;

            await _gameManager.MoveStockfishAsync();

            UpdateBoardVisuals();

            if (_gameManager.Game.IsGameOver())
            {
                EndGame();
                return;
            }
        }

        private void UpdateBoardVisuals()
        {
            foreach (var square in Squares)
            {
                square.Piece = CurrentRenderedBoard[square.Position.Row, square.Position.Column];

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
		public bool IsClassicalGame => _gameManager.Mode == GameMode.Classical && !IsReplayMode;
		public bool IsModifiedGame => _gameManager.Mode == GameMode.Modified && !IsReplayMode;
        public PlayerColor CurrentPlayer => _gameManager.Game?.CurrentPlayer ?? PlayerColor.White;

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
			Application.Current?.Dispatcher.BeginInvoke(() =>
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

            if (IsReplayMode)
            {
                _boardPositions.Clear();
                var phantomGame = new Game(PlayerColor.White, Board.Initial());
                _boardPositions.Add(phantomGame.Board.Copy());

                foreach (var moveStr in _gameReplayStore.RequestedGame.GameMoves)
                {
                    var move = MoveFormatter.StringToMove(phantomGame.Board, moveStr);
                    phantomGame.MakeMove(move);
                    _boardPositions.Add(phantomGame.Board.Copy());
                }

                _currentBoard = 0;

                ReplayForwardsCommand = new RelayCommand(o => ForwardsReplay(), o => _currentBoard < _boardPositions.Count - 1);
                ReplayBackwardsCommand = new RelayCommand(o => BackwardsReplay(), o => _currentBoard > 0);
                StopReplayCommand = new RelayCommand(o => _windowService.SwitchWindow<AppBaseViewModel>());

                _gameManager.UserColor = _gameReplayStore.RequestedGame.UserPlayedAs.Value;

                InitializeBoard();
                UpdateBoardVisuals();

                return;
            }

            _gameManager.ConfigurateGame();

            if (_gameManager.Modifiers.Any(m => m.Modifier == ModifierType.TimeLimit))
            {
                _gameManager.Game.OnModifierDataUpdated += TimersUpdated;

                var modifierStartTime = TimeSpan.FromMinutes(int.Parse(_gameManager.Modifiers.FirstOrDefault(m => m.Modifier == ModifierType.TimeLimit)?.SelectedParameter)).ToString(@"mm\:ss");
                WhiteUserTimerText = modifierStartTime;
                BlackUserTimerText = modifierStartTime;
            }

            UpdateBoardVisuals();

            _ = TriggerBotAsync();
        }

        private void BackwardsReplay()
        {
            _currentBoard--;
            _markedSquares.Clear();
            UpdateBoardVisuals();
        }

        private void ForwardsReplay()
        {
            _currentBoard++;
            _markedSquares.Clear();
            UpdateBoardVisuals();
        }

        public override void Dispose()
        {
            if (_gameManager.Modifiers.Any(m => m.Modifier == ModifierType.TimeLimit))
            {
                _gameManager.Game.OnModifierDataUpdated -= TimersUpdated;
            }

            _gameReplayStore.RequestedGame = null;

            base.Dispose();
        }
	}
}
