using Chess.Model;

namespace Chess.Data
{
    public class Repositories
    {
        public interface IUserRepository
        {
            Task<UserEntity> GetUserByUsernameAsync(string username);
            Task AddUserAsync(UserEntity newUser);
            Task DeleteUserAsync(UserEntity user);
        }
        public interface IGameRepository
        {
            Task<List<GameEntity>> GetGamesByUserAsync(UserEntity user);
            Task AddGameAsync(GameEntity newGame);
            Task DeleteAllUserGamesAsync(UserEntity user);
        }
        public interface IThemeRepository
        {
            Task<IEnumerable<ThemeEntity>> GetUserThemesAsync(UserEntity user);
            Task AddThemeAsync(ThemeEntity newTheme);
        }
        public interface IPieceThemeRepository
        {
            Task<IEnumerable<PieceThemeEntity>> GetUserThemesAsync(UserEntity user);
            Task AddThemeAsync(PieceThemeEntity newTheme);
        }
        public interface IRadioChannelRepository
        {
            Task<IEnumerable<RadioChannelEntity>> GetUserChannelsAsync(UserEntity user);
            Task AddChannelAsync(RadioChannelEntity newChannel);
        }
    }
}
