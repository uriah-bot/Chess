using Chess.Model;
using System.Data;
using System.Data.OleDb;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    public class RadioChannelRepo : IRadioChannelRepository
    {
        public async Task<IEnumerable<RadioChannelEntity>> GetDefaultChannelsAsync()
        {
            string sql = "SELECT * FROM RadioChannels WHERE UserID IS NULL";

            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql);

            List<RadioChannelEntity> channels = new List<RadioChannelEntity>();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow channel in dt.Rows)
                {
                    RadioChannelEntity entity = new RadioChannelEntity
                    {
                        Id = (int)channel["ID"],
                        ChannelPath = channel["ChannelPath"].ToString(),
                        ChannelName = channel["ChannelName"].ToString(),
                    };

                    channels.Add(entity);
                }
                return channels;
            }
            return null;
        }
    }
}
