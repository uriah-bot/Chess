using Chess.Model;
using static Chess.Data.Repositories;

namespace Chess.Service
{
    public interface IGameService
    {
        Task<List<GameEntity>> GetGamesByUserAsync(UserEntity user);
        Task SaveGameAsync(GameEntity newGame);
    }

    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepo;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepo = gameRepository;
        }

        public async Task<List<GameEntity>> GetGamesByUserAsync(UserEntity user)
        {
            return await _gameRepo.GetUserGamesAsync(user);
        }

        public Task SaveGameAsync(GameEntity newGame)
        {
            return _gameRepo.AddUserGameAsync(newGame);
        }
    }
}
