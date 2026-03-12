using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public class DoublePawnPush : Move
    {
        public override MoveType Type => MoveType.DoublePawnPush;

        public override Position FromPosition { get; }

        public override Position ToPosition { get; }

        public readonly Position skippedPosition;

        public DoublePawnPush(Position fromPosition, Position toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
            skippedPosition = new Position((fromPosition.Row + toPosition.Row) /2, fromPosition.Column);
        }
        public override bool ExecuteMove(Board board)
        {
            PlayerColor player = board[FromPosition].Color;
            board.SetPawnSkippedPosition(player, skippedPosition);
            new NormalMove(FromPosition, ToPosition).ExecuteMove(board);

            return true;
        }
    }
}
