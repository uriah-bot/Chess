using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chess.Model;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for PromotionMenu.xaml
    /// </summary>
    public partial class PromotionMenu : UserControl
    {
        public event Action<PieceType> PieceSelected;

        public PromotionMenu(PlayerColor player)
        {
            InitializeComponent();

            Color color = Color.FromArgb(255, 255, 247, 247);
            Brush whiteBrush = new SolidColorBrush(color);

            object resource = FindResource("Brush.Background");
            Brush blackBrush = resource as Brush;

            PromotionBorder.Background = player switch
            {
                PlayerColor.White => blackBrush,
                PlayerColor.Black => whiteBrush,
                _ => blackBrush
            };

            SelectionText.Foreground = player switch
            {
                PlayerColor.White => whiteBrush,
                PlayerColor.Black => blackBrush,
                _ => whiteBrush
            };

            QueenImage.Source = Images.GetImage(player, PieceType.Queen);
            RookImage.Source = Images.GetImage(player, PieceType.Rook);
            BishopImage.Source = Images.GetImage(player, PieceType.Bishop);
            KnightImage.Source = Images.GetImage(player, PieceType.Knight);
        }

        private void QueenImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(PieceType.Queen);
        }

        private void RookImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(PieceType.Rook);
        }

        private void BishopImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(PieceType.Bishop);
        }

        private void KnightImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(PieceType.Knight);
        }
    }
}
