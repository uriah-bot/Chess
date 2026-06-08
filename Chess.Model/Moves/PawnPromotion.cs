namespace Chess.Model
{
    public class PawnPromotion : Move
    {
        public override MoveType Type => MoveType.Promotion;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }
        public override Piece CapturedPiece { get; set; }

        public readonly PieceType promotedTo;

        public PawnPromotion(Position from, Position to, PieceType promotedTo)
        {
            FromPosition = from;
            ToPosition = to;
            this.promotedTo = promotedTo;
        }

        private Piece CreatePromotionPiece(PlayerColor color)
        {
            return promotedTo switch
            {
                PieceType.Queen => new Queen(color),
                PieceType.Rook => new Rook(color),
                PieceType.Bishop => new Bishop(color),
                _ => new Knight(color)
            };
        }

        public override bool ExecuteMove(Board board)
        {
            Piece pawn = board[FromPosition];
            board[FromPosition] = null;

            Piece promotedPiece = CreatePromotionPiece(pawn.Color);
            promotedPiece.HasMoved = true; // mark the promoted piece as having moved to prevent castling issues
            CapturedPiece = board[ToPosition];
            board[ToPosition] = promotedPiece;

            return true;
        }
    }
}
