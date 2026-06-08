namespace Chess.Model
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;
        public override PlayerColor Color { get; }
        private static readonly DirectionVector[] directions = new DirectionVector[]
        {
            DirectionVector.Up,
            DirectionVector.Down,
            DirectionVector.Left,
            DirectionVector.Right
        };

        public Rook(PlayerColor color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Rook Copy = new Rook(Color);
            Copy.HasMoved = HasMoved;
            return Copy;
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return MovePositionInDirections(fromPosition, board, directions).Select(toPosition => new NormalMove(fromPosition, toPosition));
        }
    }
}
