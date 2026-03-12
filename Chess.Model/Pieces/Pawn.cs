namespace Chess.Model
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override PlayerColor Color { get; }

        private readonly DirectionVector forward;
        
        public Pawn(PlayerColor color)
        {
            Color = color;
            if (color == PlayerColor.White)
            {
                forward = DirectionVector.Up;
            }
            else if (color == PlayerColor.Black)
            {
                forward = DirectionVector.Down;
            }
        }

        public override Piece Copy()
        {
            Pawn Copy = new Pawn(Color);
            Copy.HasMoved = HasMoved;
            return Copy;
        }

        private static bool CanMoveTo(Position toPosition, Board board)
        {
            return Board.IsValidPosition(toPosition) && board.IsEmptySquare(toPosition);
        }

        private bool CanCaptureAt(Position toPosition, Board board)
        {
            if (!Board.IsValidPosition(toPosition) || board.IsEmptySquare(toPosition))
            {
                return false;
            }

            return board[toPosition].Color != Color;
        }

        private static IEnumerable<Move> PromotionMovesAvailable(Position fromPosition, Position toPosition)
        {
            yield return new PawnPromotion(fromPosition, toPosition, PieceType.Knight);
            yield return new PawnPromotion(fromPosition, toPosition, PieceType.Bishop);
            yield return new PawnPromotion(fromPosition, toPosition, PieceType.Rook);
            yield return new PawnPromotion(fromPosition, toPosition, PieceType.Queen);
        }

        private IEnumerable<Move> ForwardMovesAvailable(Position fromPosition, Board board)
        {
            Position oneStepForward = fromPosition + forward;

            if (CanMoveTo(oneStepForward, board))
            {
                if (oneStepForward.Row == 0 || oneStepForward.Row == 7)
                {
                    foreach (Move promotionMove in PromotionMovesAvailable(fromPosition, oneStepForward))
                    {
                        yield return promotionMove;
                    }
                }
                else
                {
                    yield return new NormalMove(fromPosition, oneStepForward);
                }

                Position twoStepsForward = oneStepForward + forward;

                if (!HasMoved && CanMoveTo(twoStepsForward, board))
                {
                    yield return new DoublePawnPush(fromPosition, twoStepsForward);
                }
            }
        }

        private IEnumerable<Move> CapturesAvailable(Position fromPosition, Board board)
        {
            foreach (DirectionVector direction in new DirectionVector[] {DirectionVector.Left, DirectionVector.Right})
            {
                Position toPosition = fromPosition + forward + direction;

                if (toPosition == board.GetPawnSkippedPosition(Color.Opponent()))
                {
                    yield return new EnPassant(fromPosition, toPosition);
                }

                else if (CanCaptureAt(toPosition, board))
                {
                    if (toPosition.Row == 0 || toPosition.Row == 7)
                    {
                        foreach (Move promotionMove in PromotionMovesAvailable(fromPosition, toPosition))
                        {
                            yield return promotionMove;
                        }
                    }
                    else
                    {
                        yield return new NormalMove(fromPosition, toPosition);
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return ForwardMovesAvailable(fromPosition, board).Concat(CapturesAvailable(fromPosition, board));
        }

        public override bool CanCaptureOpponentKing(Position fromPosition, Board board)
        {
            return CapturesAvailable(fromPosition, board).Any(move =>
            {
                Piece piece = board[move.ToPosition];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
