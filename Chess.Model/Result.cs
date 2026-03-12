namespace Chess.Model
{
    public class Result
    {
        public PlayerColor winner { get; }
        public EndReason reason { get; }

        public Result(PlayerColor winner, EndReason reason)
        {
            this.winner = winner;
            this.reason = reason;
        }

        public static Result Win(PlayerColor player)
        {
            return new Result(player, EndReason.Checkmate);
        }

        public static Result ModifiedWin(PlayerColor player, EndReason endReason)
        {
            return new Result(player, endReason);
        }

        public static Result Draw(EndReason reason)
        {
            return new Result(PlayerColor.None, reason);
        }

        public static Result Resignation(PlayerColor resigned)
        {
            return new Result(resigned.Opponent(), EndReason.Resignation);
        }
    }
}
