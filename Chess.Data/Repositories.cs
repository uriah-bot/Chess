using Chess.Model;

namespace Chess.Data
{
    public class Repositories
    {
        // only user table needs deletion since others are tied to it via relationships with update/deletion cascade
        public interface IUserRepository
        {
            Task<List<LeaderboardEntry>> GetLeaderboardAsync(int count, string property, bool ascending = false);
            Task<UserEntity> GetUserByUsernameAsync(string username);
            Task<int> AddUserAsync(UserEntity newUser);
            Task DeleteUserAsync(UserEntity user);
            Task UpdateUserAsync(UserEntity user);
        }

        public interface IGameRepository
        {
            Task<List<GameEntity>> GetUserGamesAsync(UserEntity user);
            Task AddUserGameAsync(GameEntity newGame);
        }

        public interface IRadioChannelRepository
        {
            Task<IEnumerable<RadioChannelEntity>> GetDefaultChannelsAsync();
        }

        public interface ISettingsRepository
        {
            Task AddUserSettingsAsync(UserEntity user);
            Task UpdateUserSettingsAsync(UserEntity user);
            Task<SettingsModel> GetUserSettingAsync(UserEntity user);
        }
    }
}
