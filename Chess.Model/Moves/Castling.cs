namespace Chess.Model
{
    public class Castling : Move
    {
        public override MoveType Type { get; }

        public override Position FromPosition { get; }

        public override Position ToPosition { get; }

        public override Piece CapturedPiece
        {
            get => null;
            set { }
        }

        private readonly DirectionVector castlingDirection;
        private readonly Position rookFromPosition;
        private readonly Position rookToPosition;

        public Castling(MoveType type, Position fromPosition)
        {
            Type = type;
            FromPosition = fromPosition;

            if (type == MoveType.CastlingKing)
            {
                castlingDirection = DirectionVector.Right;
                ToPosition = new Position(fromPosition.Row, 6);
                rookFromPosition = new Position(fromPosition.Row, 7);
                rookToPosition = new Position(fromPosition.Row, 5);
            }
            else if (type == MoveType.CastlingQueen)
            {
                castlingDirection = DirectionVector.Left;
                ToPosition = new Position(fromPosition.Row, 2);
                rookFromPosition = new Position(fromPosition.Row, 0);
                rookToPosition = new Position(fromPosition.Row, 3);
            }
        }

        public override bool ExecuteMove(Board board)
        {
            new NormalMove(FromPosition, ToPosition).ExecuteMove(board);
            new NormalMove(rookFromPosition, rookToPosition).ExecuteMove(board);

            return false;
        }

        public override bool IsLegalMove(Board board)
        {
            PlayerColor player = board[FromPosition].Color;

            if (board.IsInCheck(player))
            {
                return false;
            }

            Board copy = board.Copy();
            Position kingPositionInCopy = FromPosition;

            for (int i = 0; i<2 ; i++)
            {
                new NormalMove(kingPositionInCopy, kingPositionInCopy + castlingDirection).ExecuteMove(copy);
                kingPositionInCopy += castlingDirection;

                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
