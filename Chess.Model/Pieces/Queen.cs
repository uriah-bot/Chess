namespace Chess.Model
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.Queen;
        public override PlayerColor Color { get; }
        private static readonly DirectionVector[] directions = new DirectionVector[]
        {
            DirectionVector.Up,
            DirectionVector.Down,
            DirectionVector.Left,
            DirectionVector.Right,
            DirectionVector.UpRight,
            DirectionVector.UpLeft,
            DirectionVector.DownRight,
            DirectionVector.DownLeft,
        };

        public Queen(PlayerColor color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Queen Copy = new Queen(Color);
            Copy.HasMoved = HasMoved;
            return Copy;
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return MovePositionInDirections(fromPosition, board, directions).Select(toPosition => new NormalMove(fromPosition, toPosition));
        }
    }
}
