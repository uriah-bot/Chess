using Chess.Model;
using System.Text.RegularExpressions;

namespace Chess.Service
{
    public class StockfishHelper
    {
        /*  the format for stockfish output is:
                        ex: e2e4, e1e2, h7h8q etc. 
                     unfortunately, my code isnt as smart - so it has to check for the stockfish output
                     and then make a move based on that... yeah i know right?   */

        public Move ParseStockfishMove(Board board, string stockfishOutput)
        {
            if (string.IsNullOrWhiteSpace(stockfishOutput) || stockfishOutput.Length < 4)
                throw new ArgumentException("Invalid Stockfish output.");

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

        private Move GetPromotionMove(Position fromPosition, Position toPosition, char lastLetter)
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

        private Position ChessNotationToPosition(string chessNotation)
        {
            int rank = '8' - chessNotation[1]; // row
            int file = chessNotation[0] - 'a'; // column

            return new Position(rank, file);
        }
    }
}
