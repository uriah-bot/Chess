using Chess.Model;

namespace Chess.Data
{
    public class Repositories
    {
        public interface IUserRepository
        {
            Task<UserEntity> GetUserByUsernameAndPasswordAsync(string username, string HashedPassword);
            Task AddUserAsync(UserEntity newUser);
            Task UpdateRoleAsync(string email, UserRole newRole);
        }
        public interface IGameRepository
        {
            Task<GameEntity> GetGameByIdAsync(int Id);
            Task AddGameAsync(GameEntity newGame);
        }
        public interface IThemeRepository
        {
            Task<GameEntity> GetThemesByUserIdAsync(int Id);
            Task AddThemeAsync(ThemeEntity newTheme);
        }
        public interface IPieceThemeRepository
        {
            Task<GameEntity> GetThemesByUserIdAsync(int Id);
            Task AddThemeAsync(ThemeEntity newTheme);
        }
        public interface IRadioChannelRepository
        {
            Task<GameEntity> GetChannelsByUserIdAsync(int Id);
            Task AddChannelAsync(ThemeEntity newTheme);
        }
    }
}
