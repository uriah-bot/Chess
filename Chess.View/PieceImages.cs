using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Chess.Model;

namespace Chess.View
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> WhitePieceImages = new()
        {
            { PieceType.Pawn, LoadImage("/Assets/Default/white_pawn.png") },
            { PieceType.Knight, LoadImage("/Assets/Default/white_knight.png") },
            { PieceType.Bishop, LoadImage("/Assets/Default/white_bishop.png") },
            { PieceType.Rook, LoadImage("/Assets/Default/white_rook.png") },
            { PieceType.Queen, LoadImage("/Assets/Default/white_queen.png") },
            { PieceType.King, LoadImage("/Assets/Default/white_king.png") }
        };

        private readonly static Dictionary<PieceType, ImageSource> BlackPieceImages = new()
        {
            { PieceType.Pawn, LoadImage("/Assets/Default/black_pawn.png") },
            { PieceType.Knight, LoadImage("/Assets/Default/black_knight.png") },
            { PieceType.Bishop, LoadImage("/Assets/Default/black_bishop.png") },
            { PieceType.Rook, LoadImage("/Assets/Default/black_rook.png") },
            { PieceType.Queen, LoadImage("/Assets/Default/black_queen.png") },
            { PieceType.King, LoadImage("/Assets/Default/black_king.png") }
        };

        private static ImageSource LoadImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public static ImageSource GetImage(PlayerColor color, PieceType type)
        {
            return color switch
            {
                PlayerColor.White => WhitePieceImages[type],
                PlayerColor.Black => BlackPieceImages[type],
                _ => null
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }
            return GetImage(piece.Color, piece.Type);
        }
    }
}
