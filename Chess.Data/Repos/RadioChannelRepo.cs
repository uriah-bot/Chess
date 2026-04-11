using Chess.Model;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    // TODO: ADD
    public class RadioChannelRepo : IRadioChannelRepository
    {
        public Task AddChannelAsync(RadioChannelEntity newChannel)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RadioChannelEntity>> GetUserChannelsAsync(UserEntity user)
        {
            throw new NotImplementedException();
        }
    }
}
