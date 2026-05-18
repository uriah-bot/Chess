using Chess.Model;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess.ViewModel
{
    /// Represents one square on the board.
    /// The View binds PieceImage, HighlightBrush, and OverlayBrush directly.
    public class SquareViewModel : ViewModelBase
    {
        public Position Position { get; }

        private Piece _piece;
        public Piece Piece
        {
            get => _piece;
            set
            {
                _piece = value;
                OnPropertyChanged(nameof(Piece));
            }
        }

        private Brush _highlightBrush = Brushes.Transparent;
        public Brush HighlightBrush
        {
            get => _highlightBrush;
            set
            {
                _highlightBrush = value;
                OnPropertyChanged(nameof(HighlightBrush));
            }
        }

        private Brush _overlayBrush = Brushes.Transparent;
        public Brush OverlayBrush
        {
            get => _overlayBrush;
            set
            {
                _overlayBrush = value;
                OnPropertyChanged(nameof(OverlayBrush));
            }
        }

        public SquareViewModel(int row, int column, Action<Position> OnLeftClicked, Action<Position> OnRightClicked)
        {
            Position = new Position(row, column);
            SquareLeftClickedCommand = new RelayCommand(o => OnLeftClicked(Position));
            SquareRightClickedCommand = new RelayCommand(o => OnRightClicked(Position));
        }

        public ICommand SquareLeftClickedCommand { get; set; }
        public ICommand SquareRightClickedCommand { get; }
    }
}