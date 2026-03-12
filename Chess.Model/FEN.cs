using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public class FEN
    {
        private readonly StringBuilder SB = new StringBuilder();

        public FEN(Board board, PlayerColor currentPlayer)
        {
            AddPiecePlacement(board);
            SB.Append(' ');
            AddCurrentPlayer(currentPlayer);
            SB.Append(' ');
            AddCastlingRights(board);
            SB.Append(' ');
            AddEnPassantRights(board, currentPlayer);

            SB.Append(" 0 1"); // stockfish-required
        }

        public override string ToString()
        {
            return SB.ToString();
        }

        private static char PieceChar(Piece piece)
        {
            char character = piece.Type switch
            {
                PieceType.Pawn => 'p',
                PieceType.Knight => 'n',
                PieceType.Bishop => 'b',
                PieceType.Rook => 'r',
                PieceType.Queen => 'q',
                PieceType.King => 'k',
                _ => ' '
            };

            if (piece.Color == PlayerColor.White)
            {
                return char.ToUpper(character);
            }

            return character;
        }

        private void AddRowData(Board board, int row)
        {
            int empty = 0;

            for (int col = 0; col < 8; col++)
            {
                if (board[row, col] == null)
                {
                    empty++;
                    continue;
                }

                if (empty > 0)
                {
                    SB.Append(empty);
                    empty = 0;
                }

                SB.Append(PieceChar(board[row, col]));
            }

            if (empty > 0)
            {
                SB.Append(empty);  
            }
        }

        private void AddPiecePlacement(Board board)
        {
            for (int row = 0; row < 8; row++)
            {
                if (row != 0)
                {
                    SB.Append('/');
                }

                AddRowData(board, row);
            }
        }

        private void AddCurrentPlayer(PlayerColor currentPlayer)
        {
            if (currentPlayer == PlayerColor.White)
            {
                SB.Append('w');
            }
            else
            {
                SB.Append('b');
            }
        }

        private void AddCastlingRights(Board board)
        {
            bool castleWKS = board.HasCastleRightsKingSide(PlayerColor.White);
            bool castleWQS = board.HasCastleRightsQueenSide(PlayerColor.White);
            bool castleBKS = board.HasCastleRightsKingSide(PlayerColor.Black);
            bool castleBQS = board.HasCastleRightsQueenSide(PlayerColor.Black);

            if (!(castleWKS || castleWQS || castleBKS || castleBQS)) // if all dont have
            {
                SB.Append('-');
                return;
            }

            if (castleWKS)
            {
                SB.Append('K');
            }

            if (castleWQS)
            {
                SB.Append('Q');
            }

            if (castleBKS)
            {
                SB.Append('k');
            }

            if (castleBQS)
            {
                SB.Append('q');
            }
        }

        private void AddEnPassantRights(Board board, PlayerColor currentPlayer)
        {
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                SB.Append('-');
                return;
            }

            Position skippedPosition = board.GetPawnSkippedPosition(currentPlayer.Opponent());
            char file = (char)('a' + skippedPosition.Column);
            int rank = 8 - skippedPosition.Row;
            SB.Append(file);
            SB.Append(rank);
        }
    }
}
