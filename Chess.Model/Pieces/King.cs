namespace Chess.Model
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override PlayerColor Color { get; }
        private static readonly DirectionVector[] directions = new DirectionVector[]
        {
            DirectionVector.Up,
            DirectionVector.Down,
            DirectionVector.Left,
            DirectionVector.Right,
            DirectionVector.UpLeft,
            DirectionVector.UpRight,
            DirectionVector.DownLeft,
            DirectionVector.DownRight
        };

        public King(PlayerColor color)
        {
            Color = color;
        }

        private static bool IsUnmovedRook(Position position, Board board)
        {
            if (board.IsEmptySquare(position))
            {
                return false;
            }

            Piece piece = board[position];
            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        private static bool AreAllEmptyPositions(IEnumerable<Position> positions, Board board)
        {
            return positions.All(pos => board.IsEmptySquare(pos));
        }

        private bool CanCastleKingSide(Position fromPosition, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPosition = new Position(fromPosition.Row, 7);
            Position[] inbetweenPositions = new Position[] { new Position(fromPosition.Row, 5), new Position(fromPosition.Row, 6) };

            return IsUnmovedRook(rookPosition, board) && AreAllEmptyPositions(inbetweenPositions, board);
        }

        private bool CanCastleQueenSide(Position fromPosition, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPosition = new Position(fromPosition.Row, 0);
            Position[] inbetweenPositions = new Position[] { new Position(fromPosition.Row, 3), new Position(fromPosition.Row, 2), new Position(fromPosition.Row, 1) };

            return IsUnmovedRook(rookPosition, board) && AreAllEmptyPositions(inbetweenPositions, board);
        }

        public override Piece Copy()
        {
            King Copy = new King(Color);
            Copy.HasMoved = HasMoved;
            return Copy;
        }
        
        private IEnumerable<Position> MovePositions(Position fromPosition, Board board)
        {
            foreach (DirectionVector direction in directions)
            {
                Position toPosition = fromPosition + direction;
                if (!Board.IsValidPosition(toPosition))
                {
                    continue;
                }

                if (board.IsEmptySquare(toPosition) || board[toPosition].Color != Color)
                {
                    yield return toPosition;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            foreach (Position toPosition in MovePositions(fromPosition, board))
            {
                yield return new NormalMove(fromPosition, toPosition);
            }

            if (CanCastleKingSide(fromPosition, board))
            {
                yield return new Castling(MoveType.CastlingKing, fromPosition);
            }
            if (CanCastleQueenSide(fromPosition, board))
            {
                yield return new Castling(MoveType.CastlingQueen, fromPosition);
            }
        }

        public override bool CanCaptureOpponentKing(Position fromPosition, Board board)
        {
            return GetMoves(fromPosition, board).Any(move =>
            {
                Piece piece = board[move.ToPosition];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
