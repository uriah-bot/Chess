using Chess.Model;
using System.Windows.Input;

namespace Chess.ViewModel
{
    /// Represents one square on the board.
    /// The View binds PieceImage, HighlightBrush, and OverlayBrush directly.
    public class SquareViewModel : ViewModelBase
    {
        public Position Position { get; }

        public string Coordinate { get; }

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

        private string _highlightBrush = "Transparent";
        public string HighlightBrush
        {
            get => _highlightBrush;
            set
            {
                _highlightBrush = value;
                OnPropertyChanged(nameof(HighlightBrush));
            }
        }

        private string _overlayBrush = "Transparent";
        public string OverlayBrush
        {
            get => _overlayBrush;
            set
            {
                _overlayBrush = value;
                OnPropertyChanged(nameof(OverlayBrush));
            }
        }

        public SquareViewModel(int row, int column, string coordinate, Action<Position> OnLeftClicked, Action<Position> OnRightClicked)
        {
            Position = new Position(row, column);
            SquareLeftClickedCommand = new RelayCommand(o => OnLeftClicked(Position));
            SquareRightClickedCommand = new RelayCommand(o => OnRightClicked(Position));

            Coordinate = coordinate;

            OnPropertyChanged(Coordinate);
        }

        public ICommand SquareLeftClickedCommand { get; }
        public ICommand SquareRightClickedCommand { get; }
    }
}