namespace Chess.Model
{
    public class MoveMultiplier : IModifier
    {
        private readonly int Multiplier;
        private int ConsecutiveMoveCount;
        private Game game;

        public MoveMultiplier(int? param)
        {
            if (param != null)
            {
                Multiplier = param.Value;
            }
            else
            {
                Multiplier = AppConstants.MOVE_MULTIPLIER_DEFAULT_MULTIPLIER;
            }
        }

        public void Apply(Game game)
        {
            this.game = game;
            this.game.OnPieceMoved += MultiplyMove;
        }

        public void Remove(Game game)
        {
            this.game = game;
            this.game.OnPieceMoved -= MultiplyMove;
        }

        private void MultiplyMove(Move move)
        {
            // multiplier allows one less consecutive moves (multiplier = consecutive + initial move (which is 1))
            if (ConsecutiveMoveCount >= Multiplier - 1)
            {
                ConsecutiveMoveCount = 0;
                return;
            }

            if (game.IsGameOver() || game.Board.IsInCheck(game.CurrentPlayer.Opponent()))
            {
                ConsecutiveMoveCount = 0;
                return;
            }

            game.CurrentPlayer = game.CurrentPlayer.Opponent(); // shouldnt effect anything else
            ConsecutiveMoveCount++;
        }
    }
}
