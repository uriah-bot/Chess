namespace Chess.Model
{
    public readonly record struct GameRecord
    {
        // will be used for UI
        public GameRecord(GameEntity game)
        {
            Game = game;
            GameMode = game.BotRating == null ? "Friendly Battle" : "Player vs AI";
            AIName = game.BotRating == null ? string.Join(", ", game.Modifiers.Select(m => m != ModifierType.Empty ? m.ToString() : string.Empty)) : "Stockfish (" + game.BotRating.ToString() + ")";
            UserColor = game.BotRating == null ? string.Empty : game.UserPlayedAs.ToString();
            EloDelta = game.EloDelta.HasValue && game.EloDelta >= 0 ? $"+{game.EloDelta}" : game.EloDelta.ToString();
            Result = game.BotRating == null ? "" : game.Result.ToString();
            Date = game.DatePlayed.ToString("yy-MM-dd--hh--mm");
        }

        public GameEntity Game { get; }
        public string GameMode { get; }
        public string AIName { get; }
        public string EloDelta { get; }
        public string Result { get; }
        public string Date { get; }
        public string UserColor { get; }
        public string ResultColor
        {
            get
            {
                return Result switch
                {
                    "Win" => "ForestGreen",
                    "Loss" => "MediumVioletRed",
                    "Draw" => "LightGray",
                    _ => "Gray"
                };
            }
        }
    }

    public record struct LeaderboardEntry
    {
        public LeaderboardEntry(UserEntity user)
        {
            Username = user.Username;
            Elo = user.Elo;
            Wins = user.Wins;
        }

        public LeaderboardEntry(string Username, int Elo, int Wins, bool IsCurrentUser)
        {
            this.Username = Username;
            this.Elo = Elo;
            this.Wins = Wins;
            this.IsCurrentUser = IsCurrentUser;
        }

        public string Username { get; set; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public bool IsCurrentUser { get; set; }
    }

    public record ModifierData
    {
        public string Name { get; set; }
        public string IconName { get; set; }
        public string IconHexColor { get; set; }
        public string FontFamilyName { get; set; }
        public string Type { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public bool IsDynamic { get; set; }
        public List<string> DynamicItems { get; set; }
    }
    
    // will be used for UI AND logic
    public record ActiveModifier
    {
        public ModifierType Modifier { get; set; }
        public string SelectedParameter { get; set; }
    }
}
