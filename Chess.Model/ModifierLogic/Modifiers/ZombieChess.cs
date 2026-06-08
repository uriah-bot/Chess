namespace Chess.Model
{
    public class ZombieChess : IModifier
    {
        private Game game;

        public void Apply(Game game)
        {
            this.game = game;
            this.game.OnPieceMoved += SpawnZombie;
        }

        public void Remove(Game game)
        {
            this.game = game;
            this.game.OnPieceMoved -= SpawnZombie;
        }

        private void SpawnZombie(Move move)
        {
            SpawnPhysicalPiece(move.CapturedPiece);
        }

        private void SpawnPhysicalPiece(Piece piece)
        {
            if (piece == null)
                return;

            Position position = null;
            bool condition = game.CurrentPlayer == PlayerColor.White;

            int backRank = condition ? 7 : 0;
            int step = condition ? -1 : 1;

            for (int i = backRank; i != 7 - backRank; i += step)
            {
                position = game.Board.GetEmptyPositions().Where(pos => pos.Row == i).FirstOrDefault();

                if (position != null)
                    break;
            }

            game.Board[position] = CreateZombiePiece(piece);
        }

        private static Piece CreateZombiePiece(Piece capturedPiece)
        {
            PlayerColor color = capturedPiece.Color.Opponent();

            Rook rook = new Rook(color);
            rook.HasMoved = true; // prevents castling

            return (capturedPiece) switch
            {
                Pawn => new Pawn(color),
                Knight => new Knight(color),
                Bishop => new Bishop(color),
                Rook => rook,
                Queen => new Queen(color),
                _ => throw new InvalidOperationException("Unknown piece type"),
            };
        }
    }
}
