using Chess.Model;
using Chess.Service;
using Chess.ViewModel.ViewModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class PromotionMenuViewModel : DialogViewModel
    {
        private readonly IGameManagerService _gameManager;
        private Action<PieceType> OnPieceSelected;
        public ICommand PieceSelectedCommand { get; }
        public PromotionMenuViewModel(IGameManagerService gameManager)
        {
            _gameManager = gameManager;

            PieceSelectedCommand = new RelayCommand(o => PieceSelected());
        }

        public void PieceSelected()
        {
            var move = _gameManager.LastMove;

            OnPieceSelected += pieceType =>
            {
                RequestClose?.Invoke();
                Move promotionMove = new PawnPromotion(move.FromPosition, move.ToPosition, pieceType);
            };
        }
    }
}
