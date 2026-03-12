namespace Chess.Model
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override PlayerColor Color { get; }
        public Knight(PlayerColor color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Knight Copy = new Knight(Color);
            Copy.HasMoved = HasMoved;
            return Copy;
        }
        
        private static IEnumerable<Position> PotentielToPositions(Position fromPosition)
        {
            foreach (DirectionVector verticalDirection in new DirectionVector[] { DirectionVector.Up, DirectionVector.Down })
            {
                foreach (DirectionVector horizontalDirection in new DirectionVector[] { DirectionVector.Left, DirectionVector.Right })
                {
                    yield return fromPosition + verticalDirection * 2 + horizontalDirection;
                    yield return fromPosition + verticalDirection + horizontalDirection * 2;
                }
            }
        }
        private IEnumerable<Position> MovePositions(Position fromPosition, Board board)
        {
            return PotentielToPositions(fromPosition).Where(pos => Board.IsValidPosition(pos) && (board.IsEmptySquare(pos) || board[pos].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return MovePositions(fromPosition, board).Select(toPosition => new NormalMove(fromPosition, toPosition));
        }
    }
}