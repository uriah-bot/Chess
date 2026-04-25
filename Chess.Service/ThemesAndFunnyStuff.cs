using Chess.Model;
using static Chess.Data.Repositories;

namespace Chess.Service
{    
    public interface ICustomizableDecorManager<T> where T : DBEntity
    {
        List<T> dbEntities { get; set; }

        Task GetItemsForUserAsync(UserEntity user);
        Task AddItemAsync(UserEntity user, T entity);
        Task DeleteItemAsync(UserEntity user, T entity);
    }

    public class RadioPlayer : ICustomizableDecorManager<RadioChannelEntity>
    {
        public List<RadioChannelEntity> dbEntities { get; set; } = new List<RadioChannelEntity>();

        private readonly IRadioChannelRepository _radioRepo;

        public RadioPlayer(IRadioChannelRepository radioRepo)
        {
            _radioRepo = radioRepo;
        }

        public async Task AddItemAsync(UserEntity user, RadioChannelEntity entity)
        {
            await _radioRepo.AddChannelAsync(entity);
            dbEntities.Add(entity);
        }

        public async Task DeleteItemAsync(UserEntity user, RadioChannelEntity entity)
        {
            await _radioRepo.RemoveChannelAsync(entity);
            dbEntities.Remove(entity);
        }

        public async Task GetItemsForUserAsync(UserEntity user)
        {
            dbEntities.Clear();

            var iEnu = await _radioRepo.GetUserChannelsAsync(user);

            dbEntities = iEnu.ToList();
        }
    }

    public class BoardThemeManager : ICustomizableDecorManager<BoardThemeEntity>
    {
        public List<BoardThemeEntity> dbEntities { get; set; } = new List<BoardThemeEntity>();

        private readonly IBoardThemeRepository _boardRepo;

        public BoardThemeManager(IBoardThemeRepository boardRepo)
        {
            _boardRepo = boardRepo;
        }

        public async Task AddItemAsync(UserEntity user, BoardThemeEntity entity)
        {
            await _boardRepo.AddThemeAsync(entity);
            dbEntities.Add(entity);
        }

        public async Task DeleteItemAsync(UserEntity user, BoardThemeEntity entity)
        {
            await _boardRepo.RemoveThemeAsync(entity);
            dbEntities.Remove(entity);
        }

        public async Task GetItemsForUserAsync(UserEntity user)
        {
            dbEntities.Clear();

            var iEnu = await _boardRepo.GetUserThemesAsync(user);

            dbEntities = iEnu.ToList();
        }
    }

    public class PieceThemeManager : ICustomizableDecorManager<PieceThemeEntity>
    {
        public List<PieceThemeEntity> dbEntities { get; set; } = new List<PieceThemeEntity>();

        private readonly IPieceThemeRepository _pieceThemeRepo;

        public PieceThemeManager(IPieceThemeRepository pieceThemeRepo)
        {
            _pieceThemeRepo = pieceThemeRepo;
        }

        public async Task AddItemAsync(UserEntity user, PieceThemeEntity entity)
        {
            await _pieceThemeRepo.AddThemeAsync(entity);
            dbEntities.Add(entity);
        }

        public async Task DeleteItemAsync(UserEntity user, PieceThemeEntity entity)
        {
            await _pieceThemeRepo.RemoveThemeAsync(entity);
            dbEntities.Remove(entity);
        }

        public async Task GetItemsForUserAsync(UserEntity user)
        {
            dbEntities.Clear();

            var iEnu = await _pieceThemeRepo.GetUserThemesAsync(user);

            dbEntities = iEnu.ToList();
        }
    }
}
