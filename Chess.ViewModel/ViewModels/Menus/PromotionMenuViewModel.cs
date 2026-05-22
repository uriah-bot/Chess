using Chess.Model;
using Chess.ViewModel.ViewModelHelper;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class PromotionMenuViewModel : ViewModelBase, IDialogViewModel
    {
        public Action RequestClose { get; set; }

        public string PromotionColor { get; set; }
        public string PromotionColorInverse { get; set; }
        PlayerColor PlayerColor => _gameManager.Game.CurrentPlayer;
        private readonly IGameManagerService _gameManager;
        public ICommand PieceSelectedCommand { get; }
        public ObservableCollection<Piece> AvailablePromotions { get; } = new ObservableCollection<Piece>();

        public PromotionMenuViewModel(IGameManagerService gameManager)
        {
            _gameManager = gameManager;

            PromotionColor = PlayerColor == PlayerColor.White ? "White" : "Black";
            PromotionColorInverse = PlayerColor == PlayerColor.White ? "Black" : "White";
            AddOptions(AvailablePromotions);

            PieceSelectedCommand = new RelayCommand(o => PieceSelected(o));
        }

        private void AddOptions(ObservableCollection<Piece> collection)
        {
            collection.Add(new Queen(PlayerColor));
            collection.Add(new Rook(PlayerColor));
            collection.Add(new Bishop(PlayerColor));
            collection.Add(new Knight(PlayerColor));
        }

        public void PieceSelected(object pieceType)
        {
            var PieceType = (PieceType)pieceType;
            var finalMove = _gameManager.PendingPromotionMoves.OfType<PawnPromotion>().FirstOrDefault(p => p.promotedTo == PieceType);

            if (finalMove != null)
            {
                _gameManager.MoveHuman(finalMove);
                _gameManager.PendingPromotionMoves = null;
            }

            RequestClose?.Invoke();
        }
    }
}
