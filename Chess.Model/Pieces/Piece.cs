namespace Chess.Model
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract PlayerColor Color { get; }
        public bool HasMoved { get; set; } = false;
        public abstract Piece Copy();
        public abstract IEnumerable<Move> GetMoves(Position fromPosition, Board board);
        protected IEnumerable<Position> MovePositionInDirection(Position fromPosition, Board board, DirectionVector direction)
        {
            for (Position pos = fromPosition + direction; Board.IsValidPosition(pos); pos += direction) // for sliding pieces like rook, bishop, queen
            {
                if (board.IsEmptySquare(pos))
                {
                    yield return pos;
                    continue;
                }

                Piece piece = board[pos];
                if (piece.Color != Color)
                {
                    yield return pos;
                }

                yield break;
            }
        }

        protected IEnumerable<Position> MovePositionInDirections(Position fromPosition, Board board, DirectionVector[] directions) // for sliding pieces like rook, bishop, queen
        {
            return directions.SelectMany(direction => MovePositionInDirection(fromPosition, board, direction));
        }

        public virtual bool CanCaptureOpponentKing(Position fromPosition, Board board)
        {
            return GetMoves(fromPosition, board).Any(move =>
            {
                Piece piece = board[move.ToPosition];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}