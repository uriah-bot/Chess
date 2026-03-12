namespace Chess.Model
{
    public class Counter
    {
        private Dictionary<PieceType, int> whitePieceCount = new Dictionary<PieceType, int>();
        private Dictionary<PieceType, int> blackPieceCount = new Dictionary<PieceType, int>();

        public int totalCount { get; private set; }

        public Counter()
        {
            foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
            {
                whitePieceCount[pieceType] = 0;
                blackPieceCount[pieceType] = 0;
            }
        }

        public void Increment(PlayerColor color, PieceType pieceType)
        {
            if (color == PlayerColor.White)
            {
                whitePieceCount[pieceType]++;
            }
            else if (color == PlayerColor.Black)
            {
                blackPieceCount[pieceType]++;
            }

            totalCount++;
        }

        public int WhiteCountByPiece(PieceType type)
        {
            return whitePieceCount[type];
        }

        public int BlackCountByPiece(PieceType type)
        {
            return blackPieceCount[type];
        }
    }
}
