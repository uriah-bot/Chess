using Chess.Model;
using static Chess.Data.Repositories;

namespace Chess.Service
{    
    public interface ICustomizableDecorManager<T> where T : DBEntity
    {
        List<T> dbEntities { get; set; }

        Task GetDefaultItemsAsync();
    }

    public class RadioPlayer : ICustomizableDecorManager<RadioChannelEntity>
    {
        public List<RadioChannelEntity> dbEntities { get; set; } = new List<RadioChannelEntity>();

        private readonly IRadioChannelRepository _radioRepo;

        public RadioPlayer(IRadioChannelRepository radioRepo)
        {
            _radioRepo = radioRepo;
        }

        public async Task GetDefaultItemsAsync()
        {
            dbEntities.Clear();

            var iEnu = await _radioRepo.GetDefaultChannelsAsync();

            dbEntities = iEnu.ToList();
        }
    }
}
