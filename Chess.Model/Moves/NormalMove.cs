namespace Chess.Model
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }
        public override Piece CapturedPiece { get; set; }

        public NormalMove(Position from, Position to)
        {
            FromPosition = from;
            ToPosition = to;
        }

        public override bool ExecuteMove(Board board)
        {
            Piece piece = board[FromPosition];
            bool isCapturingMove = !board.IsEmptySquare(ToPosition);
            CapturedPiece = board[ToPosition];
            board[ToPosition] = piece;
            board[FromPosition] = null;
            piece.HasMoved = true;

            return isCapturingMove || piece.Type == PieceType.Pawn;
        }
    }
}
