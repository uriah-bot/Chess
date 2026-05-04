using Chess.Model;

namespace Chess.Data
{
    public class Repositories
    {
        public interface IUserRepository
        {
            Task<List<LeaderboardEntry>> GetLeaderboardAsync(int count, string property, bool ascending = false);
            Task<UserEntity> GetUserByUsernameAsync(string username);
            Task AddUserAsync(UserEntity newUser);
            Task DeleteUserAsync(UserEntity user);
            Task UpdateUserAsync(UserEntity user);
        }
        public interface IGameRepository
        {
            Task<List<GameEntity>> GetGamesByUserAsync(UserEntity user);
            Task AddGameAsync(GameEntity newGame);
            Task DeleteAllUserGamesAsync(UserEntity user);
        }
        public interface IBoardThemeRepository
        {
            Task<IEnumerable<BoardThemeEntity>> GetUserThemesAsync(UserEntity user);
            Task AddThemeAsync(BoardThemeEntity newTheme);
            Task RemoveThemeAsync(BoardThemeEntity newTheme);
        }
        public interface IPieceThemeRepository
        {
            Task<IEnumerable<PieceThemeEntity>> GetUserThemesAsync(UserEntity user);
            Task AddThemeAsync(PieceThemeEntity newTheme);
            Task RemoveThemeAsync(PieceThemeEntity newTheme);
        }

        public interface IRadioChannelRepository
        {
            Task<IEnumerable<RadioChannelEntity>> GetDefaultChannelsAsync(UserEntity user);
        }

        public interface ISettingsRepository
        {
            Task UpdateUserSettings(UserEntity user);
            Task<SettingsModel> GetUserSetting(UserEntity user);
        }
    }
}
