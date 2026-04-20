using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public class KingPromotion : IModifier
    {
        private Game _game; // so Result can be set

        public List<ModifierType> Conflicts => null;

        public void Apply(Game game)
        {
            _game = game;

            _game.OnPieceMoved += CheckForKingPromoted;
        }

        public void Remove(Game game)
        {
            _game = game;

            _game.OnPieceMoved -= CheckForKingPromoted;
        }

        private void CheckForKingPromoted(Move move)
        {
            if (_game.Result != null)
            {
                return;
            }

            Piece piece = _game.Board[move.ToPosition];

            if (piece == null || piece.Type != PieceType.King)
            {
                return;
            }

            if (piece.Color == PlayerColor.White && move.ToPosition.Row == 0)
            {
                _game.Result = Result.ModifiedWin(PlayerColor.White, EndReason.KingPromotion);
            }

            if (piece.Color == PlayerColor.Black && move.ToPosition.Row == 7)
            {
                _game.Result = Result.ModifiedWin(PlayerColor.Black, EndReason.KingPromotion);
            }
        }
    }
}
