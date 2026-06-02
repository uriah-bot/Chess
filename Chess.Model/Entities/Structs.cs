namespace Chess.Model
{
    public record struct LeaderboardEntry
    {
        public LeaderboardEntry(UserEntity user)
        {
            Username = user.Username;
            Elo = user.Elo;
            Wins = user.Wins;
            Role = user.Role;
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
        public UserRole Role { get; set; }
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
