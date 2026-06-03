using Chess.Model;

namespace Chess.Service
{
    public static class MoveFormatter
    {
        /*  the format for stockfish output is:
                        ex: e2e4, e1e2, h7h8q etc. 
                     unfortunately, my code isnt as smart - so it has to check for the stockfish output
                     and then make a move based on that... yeah i know right?   */

        // Will be used for DataBase too, to convert chess notation to position and vice versa
        public static Move ParseStockfishMove(Board board, string stockfishOutput)
        {
            stockfishOutput = stockfishOutput.Trim();

            if (string.IsNullOrWhiteSpace(stockfishOutput) || stockfishOutput.Length < 4)
                throw new ArgumentException("Invalid Stockfish output.");
            if (stockfishOutput.Contains("(none)") || stockfishOutput.Contains("0000"))
                return null;

            string startSquare = stockfishOutput.Substring(0, 2);
            string targetSquare = stockfishOutput.Substring(2, 2);

            string[] stockfishOutputParts = { startSquare, targetSquare };

            var fromPosition = ChessNotationToPosition(startSquare);
            var toPosition = ChessNotationToPosition(targetSquare);

            var pieceType = board[fromPosition].Type;

            if (stockfishOutput.Length == 5)
            {
                char lastLetter = stockfishOutput[stockfishOutput.Length - 1];
                return GetPromotionMove(fromPosition, toPosition, lastLetter);
            }

            if (pieceType == PieceType.Pawn
                && board.IsEmptySquare(toPosition) && startSquare[0] != targetSquare[0])
            {
                return new EnPassant(fromPosition, toPosition);
            }

            if (pieceType == PieceType.Pawn
                && Math.Abs(targetSquare[1] - startSquare[1]) == 2)
            {
                return new DoublePawnPush(fromPosition, toPosition);
            }

            if (pieceType == PieceType.King)
            {
                if (startSquare[0] + 2 == targetSquare[0])
                {
                    return new Castling(MoveType.CastlingKing, fromPosition);
                }

                if (startSquare[0] - 2 == targetSquare[0])
                {
                    return new Castling(MoveType.CastlingQueen, fromPosition);
                }
            }
            
            return new NormalMove(fromPosition, toPosition);
        }

        public static (Move, Position) StringToMove(Board board, string move)
        {
            string startSquare = move.Substring(0, 2);
            string targetSquare = move.Substring(4, 2);

            var fromPosition = new Position(int.Parse(startSquare[0].ToString()), int.Parse(startSquare[1].ToString()));
            var toPosition = new Position(int.Parse(targetSquare[0].ToString()), int.Parse(targetSquare[1].ToString()));

            Position poofedPos = null;
            var pieceType = board[fromPosition].Type;

            if (move.Contains('#'))
            {
                string[] moveParts = move.Split('#');
                move = moveParts[0];
                poofedPos = new Position(int.Parse(moveParts[1][0].ToString()), int.Parse(moveParts[1][1].ToString()));
            }

            if (move.Contains("="))
            {
                char lastLetter = move.Last();
                return (GetPromotionMove(fromPosition, toPosition, lastLetter), poofedPos);
            }

            if (pieceType == PieceType.Pawn
                && board.IsEmptySquare(toPosition) && startSquare[0] != targetSquare[0])
            {
                return (new EnPassant(fromPosition, toPosition), poofedPos);
            }

            if (pieceType == PieceType.Pawn
                && Math.Abs(targetSquare[1] - startSquare[1]) == 2)
            {
                return (new DoublePawnPush(fromPosition, toPosition), poofedPos);
            }

            if (pieceType == PieceType.King)
            {
                if (startSquare[0] + 2 == targetSquare[0])
                {
                    return (new Castling(MoveType.CastlingKing, fromPosition), poofedPos);
                }

                if (startSquare[0] - 2 == targetSquare[0])
                {
                    return (new Castling(MoveType.CastlingQueen, fromPosition), poofedPos);
                }
            }
            
            return (new NormalMove(fromPosition, toPosition), poofedPos);
        }

        private static PawnPromotion GetPromotionMove(Position fromPosition, Position toPosition, char lastLetter)
        {
            var newPieceType = lastLetter switch
            {
                'q' => PieceType.Queen,
                'r' => PieceType.Rook,
                'n' => PieceType.Knight,
                'b' => PieceType.Bishop,
                _ => PieceType.Queen,
            };

            return new PawnPromotion(fromPosition, toPosition, newPieceType);
        }

        private static char GetPromotedPieceType(PawnPromotion move)
        {
            var newPieceType = move.promotedTo switch
            {
                PieceType.Queen => 'q',
                PieceType.Rook => 'r',
                PieceType.Knight => 'n',
                PieceType.Bishop => 'b',
                _ => 'q',
            };

            return newPieceType;
        }

        private static Position ChessNotationToPosition(string chessNotation)
        {
            int rank = '8' - chessNotation[1]; // row
            int file = chessNotation[0] - 'a'; // column

            return new Position(rank, file);
        }

        public static string MoveToString(Move move)
        {
            var fromRow = move.FromPosition.Row.ToString();
            var fromCol = move.FromPosition.Column.ToString();
            var toRow = move.ToPosition.Row.ToString();
            var toCol = move.ToPosition.Column.ToString();

            return $"{fromRow}{fromCol}->{toRow}{toCol}{(move is PawnPromotion ? $"={GetPromotedPieceType((PawnPromotion)move)}" : string.Empty)}";
        }
    }
}
