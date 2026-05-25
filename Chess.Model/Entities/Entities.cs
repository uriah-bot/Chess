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
        public SettingsModel Settings { get; set;}
    }

    public class GameEntity : DBEntity
    {
        public int UserId { get; set; }
        public int? EloDelta { get; set; }
        public List<string> GameMoves { get; set; } = new List<string>();
        public PlayerColor? UserPlayedAs { get; set; }
        public int? BotRating { get; set; }
        public string Result { get; set; } // playercolor -> User
        public DateTime DatePlayed { get; set; }
        public List<ModifierType> Modifiers { get; set; }
    }

    public class RadioChannelEntity : DBEntity
    {
        public int? UserId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelPath { get; set; }
        public bool IsSelected { get; set; }
    }

    public class SettingsModel : DBEntity
    {
        public int UserId { get; set; }
        public bool SoundEffectOnMove { get; set; }
        public double Volume { get; set; } = AppConstants.DEFAULT_VOLUME;
        public string CurrentSong { get; set; } = "DefaultMusic.mp3"; // the path
        public bool StopRadioOnMatches { get; set; }
        public bool DisplayCoordinates { get; set; }
    }
}
