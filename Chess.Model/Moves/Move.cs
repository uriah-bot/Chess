namespace Chess.Model
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPosition { get; }
        public abstract Position ToPosition { get; }
        public abstract bool ExecuteMove(Board board);

        public virtual bool IsLegalMove(Board board)
        {
            PlayerColor playerColor = board[FromPosition].Color;
            Board copy = board.Copy();
            ExecuteMove(copy);
            return !copy.IsInCheck(playerColor);
        }
    }
}
