using Chess.Model;
using Chess.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ViewModel.Stores
{
    public interface IGameHistoryStore
    {
        // TODO: Change to service
        ObservableCollection<GameEntity> UserGames { get; }
        Task LoadGamesAsync();
        Task SaveGameAsync(GameEntity newGame);
    }

    public class GameHistoryStore : IGameHistoryStore
    {
        private readonly IUserStore _userStore;
        private readonly IGameService _gameService;

        public ObservableCollection<GameEntity> UserGames { get; } = new ObservableCollection<GameEntity>();

        public GameHistoryStore(IUserStore userStore, IGameService gameService)
        {
            _userStore = userStore;
            _gameService = gameService;
        }

        public async Task LoadGamesAsync()
        {
            if (_userStore.CurrentUser == null) return;

            UserGames.Clear();

            var games = await _gameService.GetGamesByUserAsync(_userStore.CurrentUser);

            if (games != null)
            {
                foreach (var game in games)
                {
                    UserGames.Add(game);
                }
            }
        }

        public async Task SaveGameAsync(GameEntity newGame)
        {
            await _gameService.SaveGameAsync(newGame);
            UserGames.Add(newGame);
        }
    }
}
