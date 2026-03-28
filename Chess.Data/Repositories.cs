using Chess.Model;

namespace Chess.Data
{
    public class Repositories
    {
        public interface IUserRepository
        {
            Task<UserEntity> GetUserByUsernameAsync(string username);
            Task AddUserAsync(UserEntity newUser);
            Task UpdateRoleAsync(string username, UserRole newRole);
            Task UpdateEloAsync(string username, int updatedElo);
        }
        public interface IGameRepository
        {
            Task<List<GameEntity>> GetGamesByUsernameAsync(string username);
            Task AddGameAsync(GameEntity newGame);
        }
        public interface IThemeRepository
        {
            Task<IEnumerable<ThemeEntity>> GetThemesByUserIdAsync(int Id);
            Task AddThemeAsync(ThemeEntity newTheme);
        }
        public interface IPieceThemeRepository
        {
            Task<IEnumerable<PieceThemeEntity>> GetThemesByUserIdAsync(int Id);
            Task AddThemeAsync(PieceThemeEntity newTheme);
        }
        public interface IRadioChannelRepository
        {
            Task<IEnumerable<RadioChannelEntity>> GetChannelsByUserIdAsync(int Id);
            Task AddChannelAsync(RadioChannelEntity newChannel);
        }
    }
}
