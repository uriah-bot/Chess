namespace Chess.Model
{
    public enum PlayerColor
    {
        None,
        White,
        Black
    }

    public static class PlayerExtensions
    {
        public static PlayerColor Opponent(this PlayerColor player)
        {
            return player switch
            {
                PlayerColor.White => PlayerColor.Black,
                PlayerColor.Black => PlayerColor.White,
                _ => PlayerColor.None
            };
        }
    }
}
