using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public class Game
    {
        private readonly List<IModifier> ActiveModifiers = new List<IModifier>();

        public delegate void PieceMovedHandler(Move move);
        public event PieceMovedHandler OnPieceMoved;

        public delegate void BoardSetupHandler(Board board);
        public event BoardSetupHandler OnBoardSetup;

        public int HalfMoves { get; private set; } = 0; // modifiers

        public Board Board { get; }
        public PlayerColor CurrentPlayer { get; private set; }
        public Result Result { get; set; } = null;

        private int noCaptureOrPawnMove = 0;
        private string gameStateString;

        private readonly Dictionary<string, int> stateHistory = new Dictionary<string, int>();


        public event Action<string, string> OnModifierDataUpdated;
        public void BroadcastModifierData(string key, string value)
        {
            OnModifierDataUpdated?.Invoke(key, value);
        }

        // white always starts, this constructor helps with testing
        public Game(PlayerColor player, Board board)
        {
            Board = board;
            CurrentPlayer = player;

            gameStateString = new FEN(board, CurrentPlayer).ToString();
            stateHistory[gameStateString] = 1;
        }

        private void ApplyModifiers(List<ModifierType> selectedModifiers)
        {
            if (selectedModifiers.Count == 0) return;

            foreach (var modType in selectedModifiers)
            {
                IModifier modifier = ModifierFactory.Create(modType);
                if (modifier != null)
                {
                    ActiveModifiers.Add(modifier);
                    modifier.Apply(this);
                }
            }
        }

        public void RemoveModifiers()
        {
            foreach (var modifier in ActiveModifiers)
            {
                if (modifier != null)
                {
                    ActiveModifiers.Remove(modifier);
                    modifier.Remove(this);
                }
            }
        }

        public void StartMatch(List<ModifierType> selectedModifiers)
        {
            ApplyModifiers(selectedModifiers);
            OnBoardSetup?.Invoke(Board);
        }

        public IEnumerable<Move> LegalMovesForPiece(Position position)
        {
            if (Board.IsEmptySquare(position) || Board[position].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[position];
            IEnumerable<Move> potentialMoves = piece.GetMoves(position, Board);
            return potentialMoves.Where(move => move.IsLegalMove(Board));
        }

        public void MakeMove(Move move)
        {
            Board.SetPawnSkippedPosition(CurrentPlayer, null);
            bool captureOrPawnMove = move.ExecuteMove(Board);
            HalfMoves++;

            OnPieceMoved?.Invoke(move);

            if (IsGameOver()) // some modifiers set the Result
            {
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
            if (IsGameOver()) // guard for modifiers
            {
                return;
            }

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

        public void HasResigned()
        {
            Result = Result.Resignation(CurrentPlayer);
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
