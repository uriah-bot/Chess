namespace Chess.Model
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;
        public override PlayerColor Color { get; }
        private static readonly DirectionVector[] directions = new DirectionVector[]
        {
            DirectionVector.UpRight,
            DirectionVector.UpLeft,
            DirectionVector.DownRight,
            DirectionVector.DownLeft,
        };

        public Bishop(PlayerColor color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Bishop Copy = new Bishop(Color);
            Copy.HasMoved = HasMoved;
            return Copy;
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return MovePositionInDirections(fromPosition, board, directions).Select(toPosition => new NormalMove(fromPosition, toPosition));
        }
    }
}
