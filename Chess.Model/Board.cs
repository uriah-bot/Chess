using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];

        private readonly Dictionary<PlayerColor, Position> pawnSkippedPositions = new Dictionary<PlayerColor, Position>()
        {
            { PlayerColor.White, null },
            { PlayerColor.Black, null }
        };

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }

        public Position GetPawnSkippedPosition(PlayerColor player)
        {
            return pawnSkippedPositions[player];
        }

        public void SetPawnSkippedPosition(PlayerColor player, Position position)
        {
            pawnSkippedPositions[player] = position;
        }

        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        // cant initialize inverse because logic is hardcoded for logicPieces to be on a regular position
        // must flip only in UI when black pieces are chosen

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(PlayerColor.Black);
            this[0, 1] = new Knight(PlayerColor.Black);
            this[0, 2] = new Bishop(PlayerColor.Black);
            this[0, 3] = new Queen(PlayerColor.Black);
            this[0, 4] = new King(PlayerColor.Black);
            this[0, 5] = new Bishop(PlayerColor.Black);
            this[0, 6] = new Knight(PlayerColor.Black);
            this[0, 7] = new Rook(PlayerColor.Black);

            this[7, 0] = new Rook(PlayerColor.White);
            this[7, 1] = new Knight(PlayerColor.White);
            this[7, 2] = new Bishop(PlayerColor.White);
            this[7, 3] = new Queen(PlayerColor.White);
            this[7, 4] = new King(PlayerColor.White);
            this[7, 5] = new Bishop(PlayerColor.White);
            this[7, 6] = new Knight(PlayerColor.White);
            this[7, 7] = new Rook(PlayerColor.White);

            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new Pawn(PlayerColor.Black);
                this[6, i] = new Pawn(PlayerColor.White);
            }
        }

        public static bool IsValidPosition(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool IsEmptySquare(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> GetPiecesPositions()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Position position = new Position(row, col);

                    if (!IsEmptySquare(position))
                    {
                        yield return position;
                    }
                }
            }
        }

        public IEnumerable<Position> GetPiecesPositionForPlayer(PlayerColor playerColor)
        {
            return GetPiecesPositions().Where(position => this[position].Color == playerColor);
        }

        public bool IsInCheck(PlayerColor playerColor)
        {
            return GetPiecesPositionForPlayer(playerColor.Opponent()).Any(position =>
            {
                Piece piece = this[position];
                return piece.CanCaptureOpponentKing(position, this);
            });
        }

        public Board Copy()
        {
            Board copy = new Board();
            foreach (Position position in GetPiecesPositions())
            {
                copy[position] = this[position].Copy();
            }
            return copy;
        }

        public Counter CountPieces()
        {
            Counter counter = new Counter();

            foreach (Position position in GetPiecesPositions())
            {
                Piece piece = this[position];
                counter.Increment(piece.Color, piece.Type);
            }

            return counter;
        }

        public bool IsInsufficientMaterial()
        {
            Counter counter = CountPieces();

            return IsKingVsKing(counter) || IsKingAndMinorPieceVsKing(counter) ||
                   IsKingAndMinorPieceVsKingAndMinorPiece(counter) || IsOtherNonForcedCheckmates(counter);
        }

        private bool IsUnmovedKingAndRook(Position kingPosition, Position rookPosition)
        {
            if (IsEmptySquare(kingPosition) || IsEmptySquare(rookPosition))
            {
                return false;
            }

            Piece king = this[kingPosition];
            Piece rook = this[rookPosition];

            // isnt required to check for piece type but may be helpful for modifiers later
            return king.Type == PieceType.King && rook.Type == PieceType.Rook &&
                   !king.HasMoved && !rook.HasMoved;
        }

        public bool HasCastleRightsKingSide(PlayerColor player)
        {
            return player switch
            {
                PlayerColor.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)),
                PlayerColor.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)),
                _ => false
            };
        }

        public bool HasCastleRightsQueenSide(PlayerColor player)
        {
            return player switch
            {
                PlayerColor.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)),
                PlayerColor.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)),
                _ => false
            };
        }

        private bool HasPawnInCapturingPositions(PlayerColor player, Position[] capturingPawnPositions, Position skippedPosition)
        {
            foreach (Position position in capturingPawnPositions.Where(IsValidPosition))
            {
                Piece piece = this[position];
                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                {
                    continue;
                }

                EnPassant move = new EnPassant(position, skippedPosition);
                if (move.IsLegalMove(this))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanCaptureEnPassant(PlayerColor player)
        {
            Position skippedPosition = GetPawnSkippedPosition(player.Opponent());

            if (skippedPosition == null)
            {
                return false;
            }

            Position[] capturingPawnPositions = player switch
            {
                PlayerColor.White => new Position[] { skippedPosition + DirectionVector.DownLeft, skippedPosition + DirectionVector.DownRight },
                PlayerColor.Black => new Position[] { skippedPosition + DirectionVector.UpLeft, skippedPosition + DirectionVector.UpRight },
                _ => Array.Empty<Position>()
            };

            return HasPawnInCapturingPositions(player, capturingPawnPositions, skippedPosition);
        }

        // Helper methods for ways the game has Insufficient Material

        /*  
            note: minor piece = Knight/Bishop
            opt. = option = methods needed (for each piece - will check both colors)

            1. King vs King - 1 opt.
            2. King + minor piece vs King - 2 opt.
            3. King + minor piece vs King + minor piece - 3 opt. (isnt forced)
            4. King + 2 Knights vs King - 1 opt. (isnt forced)
            5. King + 2 Bishops same color vs King - 1 opt. (same as 1 bishop)  */

        private static bool IsKingVsKing(Counter counter)
        {
            return counter.totalCount == 2; // must be the kings
        }

        private static bool IsKingBishopVsKing(Counter counter)
        {
            return
                counter.totalCount == 3 && 
                (counter.WhiteCountByPiece(PieceType.Bishop) == 1 || counter.BlackCountByPiece(PieceType.Bishop) == 1); 
            // must be King King + piece, and piece may be a Bishop
        }

        private static bool IsKingKnightVsKing(Counter counter)
        {
            return
                counter.totalCount == 3 &&
                (counter.WhiteCountByPiece(PieceType.Knight) == 1 || counter.BlackCountByPiece(PieceType.Knight) == 1);
            // must be King King + piece, and piece may be a Knight
        }

        private static bool IsKingAndMinorPieceVsKing(Counter counter) // cleaner
        {
            return  IsKingBishopVsKing(counter) || IsKingKnightVsKing(counter);
        }

        private static bool IsKingKnightVsKingKnight(Counter counter)
        {
            if (counter.totalCount != 4)
            {
                return false;
            }

            if (counter.WhiteCountByPiece(PieceType.Knight) != 1 || counter.BlackCountByPiece(PieceType.Knight) != 1)
            {
                return false;
            }

            return true;
        }

        private static bool IsKingBishopVsKingBishop(Counter counter)
        {
            if (counter.totalCount != 4)
            {
                return false;
            }

            if (counter.WhiteCountByPiece(PieceType.Bishop) != 1 || counter.BlackCountByPiece(PieceType.Bishop) != 1)
            {
                return false;
            }

            return true;
        }

        private static bool IsKingKnightVsKingBishop(Counter counter)
        {
            if (counter.totalCount != 4)
            {
                return false;
            }

            if ((counter.WhiteCountByPiece(PieceType.Knight) != 1 || counter.BlackCountByPiece(PieceType.Bishop) != 1) &&
                (counter.WhiteCountByPiece(PieceType.Bishop) != 1 || counter.BlackCountByPiece(PieceType.Knight) != 1))
            {
                return false;
            }

            return true;
        }

        private static bool IsKingAndMinorPieceVsKingAndMinorPiece(Counter counter) // cleaner
        {
            return IsKingBishopVsKingBishop(counter) || IsKingKnightVsKingKnight(counter) || IsKingKnightVsKingBishop(counter);
        }

        private static bool IsKingTwoKnightsVsKing(Counter counter)
        {
            if (counter.totalCount != 4)
            {
                return false;
            }

            if (counter.WhiteCountByPiece(PieceType.Knight) == 2 && counter.BlackCountByPiece(PieceType.Knight) == 0 ||
                counter.BlackCountByPiece(PieceType.Knight) == 2 && counter.WhiteCountByPiece(PieceType.Knight) == 0) 
            {
                return true;
            }

            return false;
        }

        private bool IsKingTwoBishopsSameColorVsKing(Counter counter)
        {
            if (counter.totalCount != 4)
            {
                return false;
            }

            if (counter.WhiteCountByPiece(PieceType.Bishop) != 2 && counter.BlackCountByPiece(PieceType.Bishop) != 2)
            {
                return false;
            }

            Position[] bishopPositions = FindAllPiecesByType(PieceType.Bishop);

            return bishopPositions[0].SquareColor() == bishopPositions[1].SquareColor() && this[bishopPositions[0]].Color == this[bishopPositions[1]].Color;
        }

        private bool IsOtherNonForcedCheckmates(Counter counter) // cleaner
        {
            return IsKingTwoKnightsVsKing(counter) || IsKingTwoBishopsSameColorVsKing(counter);
        }

        private Position[] FindAllPiecesByType(PieceType type)
        {
            return GetPiecesPositions().Where(position => this[position].Type == type).ToArray();
        }
    }
}
