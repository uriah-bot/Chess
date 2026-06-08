namespace Chess.Model
{
    public class Poof : IModifier
    {
        private Game _game; // so Result can be set
        private readonly Random rnd = new Random();
        private readonly int PoofCycle;
        
        public Poof(int? param)
        {
            PoofCycle = param ?? AppConstants.POOF_DEFAULT_MOVES;
        }

        public void Apply(Game game)
        {
            _game = game;

            _game.OnPieceMoved += PoofPiece;
        }

        public void Remove(Game game)
        {
            _game = game;

            _game.OnPieceMoved -= PoofPiece;
        }

        public void PoofPiece(Move move)
        {
            if (_game.HalfMoves % (2*PoofCycle) != 2*PoofCycle-1 && _game.HalfMoves % (2*PoofCycle) != 0)
            {
                return;
            }

            if (CheckForNoPieces(move, _game.CurrentPlayer))
            {
                _game.Result = Result.ModifiedWin(_game.CurrentPlayer.Opponent(), EndReason.NotEnoughPoofPieces);
                return;
            }

            List<Position> playerPiecePositions =
                _game.Board.GetPiecesPositionForPlayer(_game.CurrentPlayer).Where(pos => CanPoof(move, _game.Board[pos])).ToList();

            int posIndex = rnd.Next(playerPiecePositions.Count());
            Position positionToPoof = playerPiecePositions[posIndex];

            _game.LastPoofedPiece = positionToPoof;
            
            _game.Board[positionToPoof] = null;
        }

        private bool CheckForNoPieces(Move move, PlayerColor currentPlayer)
        {
            return !_game.Board.GetPiecesPositionForPlayer(currentPlayer).Where(pos => CanPoof(move, _game.Board[pos])).Any();
        }

        private bool CanPoof(Move lastMove, Piece piece)
        {
            if (piece == null)
            {
                return false;
            }

            // incase promoted to a poofable on the last move
            if (lastMove is PawnPromotion promotion)
            {
                return piece == _game.Board[lastMove.ToPosition] && promotion.promotedTo != PieceType.Queen;
            }

            return piece.Type != PieceType.King && piece.Type != PieceType.Pawn &&
                    piece.Type != PieceType.Queen && piece != _game.Board[lastMove.ToPosition]; // must be b4 changing player
            // must be a piece of type: Bishop/Knight/Rook
        }
    }
}
