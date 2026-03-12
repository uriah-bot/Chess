using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public class EnPassant : Move
    {
        public override MoveType Type => MoveType.EnPassant;

        public override Position FromPosition { get; }

        public override Position ToPosition { get; } // the skipped position

        private readonly Position capturePosition; // the position the pawn is in after a double-push

        public EnPassant(Position fromPosition, Position toPosition)
        {
            ToPosition = toPosition;
            FromPosition = fromPosition;
            capturePosition = new Position(fromPosition.Row, toPosition.Column);
        }

        public override bool ExecuteMove(Board board)
        {
            new NormalMove(FromPosition, ToPosition).ExecuteMove(board);
            board[capturePosition] = null;

            return true;
        }
    }
}
