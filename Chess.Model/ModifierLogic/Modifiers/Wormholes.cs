namespace Chess.Model
{
    public class Wormholes : IModifier
    {
        private Game game;
        private List<Wormhole> wormholePairs = new List<Wormhole>();
        private readonly int wormholeCount;
        private readonly Random rnd = new();
        public IEnumerable<Position> PortalPositions => wormholePairs.Select(w => w.Position);

        public Wormholes(int? param)
        {
            wormholeCount = param ?? AppConstants.WORMHOLES_DEFAULT_PORTALS;
        }

        public void Apply(Game game)
        {
            this.game = game;
            this.game.OnBoardSetup += ApplyWormholes;
            this.game.OnPieceMoved += MoveByWormhole;
            this.game.OnLegalMovesCalculated += ConfirmLegalMoves;
        }

        public void Remove(Game game)
        {
            this.game = game;
            this.game.OnBoardSetup -= ApplyWormholes;
            this.game.OnPieceMoved -= MoveByWormhole;
            this.game.OnLegalMovesCalculated -= ConfirmLegalMoves;
        }

        private IEnumerable<Move> ConfirmLegalMoves(Position position, IEnumerable<Move> legalMoves)
        {
            return legalMoves.Where(m => IsOkayMove(m));
        }

        private void ApplyWormholes(Board board)
        {
            var emptyPositions = board.GetEmptyPositions().ToList();

            for (int i = 0; i < wormholeCount; i++)
            {
                int index = rnd.Next(emptyPositions.Count);
                wormholePairs.Add(new Wormhole(emptyPositions[index]));

                emptyPositions.RemoveAt(index);
            }

            wormholePairs.ForEach(w =>
            {
                if (w.TeleportTo == null)
                {
                    var target = wormholePairs.FirstOrDefault(w1 => !w1.IsTeleportedTo && w1 != w);

                    if (target != null)
                    {
                        w.SetConnected(target);
                        target.SetConnected(w);
                    }
                }
            });
        }

        private void MoveByWormhole(Move move)
        {
            if (!wormholePairs.Any(w => w.Position == move.ToPosition))
                return;

            Teleport(move, game.Board);
        }

        private void Teleport(Move move, Board board)
        {
            if (move.ToPosition == null)
                return;

            Position toPos = move.ToPosition;
            Wormhole toWhere = wormholePairs.First(w => w.Position == toPos).TeleportTo;
            board[toWhere.Position] = board[toPos];
            board[toPos] = null;
        }

        private bool IsOkayMove(Move potentialMove)
        {
            if (!wormholePairs.Any(w => w.Position == potentialMove.ToPosition))
                return true;

            var testBoard = game.Board.Copy();
            potentialMove.ExecuteMove(testBoard);
            if (testBoard.IsInCheck(game.CurrentPlayer))
                return false;

            testBoard = game.Board.Copy();
            potentialMove.ExecuteMove(testBoard);
            Teleport(potentialMove, testBoard);

            return !testBoard.IsInCheck(game.CurrentPlayer);
        }

        public class Wormhole
        {
            public Position Position { get; set; }
            public Wormhole TeleportTo { get; private set; }
            public bool IsTeleportedTo { get; private set; }

            public Wormhole(Position pos)
            {
                Position = pos;
            }

            public void SetConnected(Wormhole to)
            {
                TeleportTo = to;
                IsTeleportedTo = true;
            }
        }
    }
}
