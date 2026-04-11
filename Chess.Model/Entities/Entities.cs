namespace Chess.Model
{
    public abstract class DBEntity
    {
        public int Id { get; set; }
    }

    public class UserEntity : DBEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public int Elo { get; set; } = AppConstants.DEFAULT_ELO;
        public int Wins { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int PeakElo { get; set; } = AppConstants.DEFAULT_ELO;
        public UserRole Role { get; set; } // Role.ToString() for db
    }

    public class GameEntity : DBEntity
    {
        public string Username { get; set; }
        public List<string> GameFENs { get; set; }
        public GameMode GameMode { get; set; }
        public PlayerColor? UserPlayedAs { get; set; }
        public int? BotRating { get; set; }
        public string Result { get; set; } // playercolor -> User
        public DateTime DatePlayed { get; set; }
    }

    public class ThemeEntity : DBEntity
    {
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class PieceThemeEntity : DBEntity
    {
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class RadioChannelEntity : DBEntity
    {
        public int? UserId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelPath { get; set; }
    }
}
