namespace Chess.Model
{
    public class Game
    {
        public Board Board { get; }
        public PlayerColor CurrentPlayer { get; internal set; }
        public event Action OnGameEndedByTimer;
        private Result _result;
        public Result Result
        {
            get => _result;
            set
            {
                if (value != null && _result == null)
                {
                    _result = value;
                    if (Result.reason == EndReason.TimeRanOut)
                    {
                        OnGameEndedByTimer?.Invoke();
                    }
                }
            }
        }
        private int noCaptureOrPawnMove = 0;
        private string gameStateString;
        private readonly Dictionary<string, int> stateHistory = new Dictionary<string, int>();

        // modifiers
        public delegate void PieceMovedHandler(Move move);
        public event PieceMovedHandler OnPieceMoved;
        public delegate void BoardSetupHandler(Board board);
        public event BoardSetupHandler OnBoardSetup;
        public int HalfMoves { get; private set; } = 0; // modifiers
        public Position LastPoofedPiece { get; set; } = null; // modifiers

        private readonly List<IModifier> ActiveModifiers = new List<IModifier>();
        public event Action<string, string> OnModifierDataUpdated;

        // white always starts, this constructor helps with testing
        public Game(PlayerColor player, Board board)
        {
            Board = board;
            CurrentPlayer = player;

            gameStateString = new FEN(board, CurrentPlayer).ToString();
            stateHistory[gameStateString] = 1;
        }

        public void BroadcastModifierData(string key, string value)
        {
            OnModifierDataUpdated?.Invoke(key, value);
        }

        private void ApplyModifiers(List<ActiveModifier> selectedModifiers)
        {
            if (selectedModifiers.Count == 0 || selectedModifiers == null) return;

            foreach (var mod in selectedModifiers)
            {
                IModifier modifier = ModifierFactory.Create(mod.Modifier, mod.SelectedParameter);
                if (modifier != null)
                {
                    ActiveModifiers.Add(modifier);
                    modifier.Apply(this);
                }
            }
        }

        private void RemoveModifiers()
        {
            if (ActiveModifiers.Count == 0 || ActiveModifiers == null) return;

            foreach (var mod in ActiveModifiers)
            {
                mod.Remove(this);
            }
        }

        public void StartMatch(List<ActiveModifier> selectedModifiers)
        {
            ApplyModifiers(selectedModifiers);
            OnBoardSetup?.Invoke(Board);
        }

        public void EndMatch()
        {
            RemoveModifiers();
        }

        public IEnumerable<Move> LegalMovesForPiece(Position position)
        {
            if (Board.IsEmptySquare(position) || Board[position].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[position];
            IEnumerable<Move> potentialMoves = piece.GetMoves(position, Board);
            return potentialMoves = potentialMoves.Where(move => move.IsLegalMove(Board));
        }

        public void MakeMove(Move move)
        {
            LastPoofedPiece = null;
            Board.SetPawnSkippedPosition(CurrentPlayer, null);
            bool captureOrPawnMove = move.ExecuteMove(Board);
            HalfMoves++;

            OnPieceMoved?.Invoke(move);

            if (IsGameOver()) // some modifiers set the Result
            {
                CurrentPlayer = Result.reason == EndReason.KingPromotion && ActiveModifiers.Any(m => m is MoveMultiplier) ? CurrentPlayer.Opponent() : CurrentPlayer;
                // if the modifiers have both, the game might end in promotion and the UI will display incorrectly
                return;
            }

            if (captureOrPawnMove)
            {
                noCaptureOrPawnMove = 0;
                stateHistory.Clear();
            }
            else
            {
                noCaptureOrPawnMove++;
            }

            CurrentPlayer = CurrentPlayer.Opponent();
            UpdateStateString();
            CheckForGameEnd();
        }

        public IEnumerable<Move> GetAllLegalMovesFor(PlayerColor playerColor)
        {
            IEnumerable<Move> potentialMoves = Board.GetPiecesPositionForPlayer(playerColor).SelectMany(position =>
            {
                Piece piece = Board[position];
                return piece.GetMoves(position, Board);
            });

            return potentialMoves.Where(move => move.IsLegalMove(Board));
        }

        public void CheckForGameEnd()
        {
            if (!GetAllLegalMovesFor(CurrentPlayer).Any())
            {
                if (Board.IsInCheck(CurrentPlayer))
                {
                    Result = Result.Win(CurrentPlayer.Opponent());
                }
                else
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }

            else if (Board.IsInsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }

            else if (IsThreefoldRepetition())
            {
                Result = Result.Draw(EndReason.ThreefoldRepetition);
            }

            else if (IsFiftyMoveRule())
            {
                Result = Result.Draw(EndReason.FiftyMoveRule);
            }
        }

        public void Resign(PlayerColor resigning)
        {
            Result = Result.Resignation(resigning);
        }

        public bool IsGameOver()
        {
            return Result != null;
        }

        private bool IsFiftyMoveRule()
        {
            int fullMoves = noCaptureOrPawnMove / 2;
            return fullMoves == 50;
        }

        private void UpdateStateString()
        {
            gameStateString = new FEN(Board,CurrentPlayer).ToString();

            if (!stateHistory.ContainsKey(gameStateString))
            {
                stateHistory[gameStateString] = 1;
            }
            else
            {
                stateHistory[gameStateString]++;
            }
        }

        private bool IsThreefoldRepetition()
        {
            return stateHistory[gameStateString] == 3;
        }
    }
}
