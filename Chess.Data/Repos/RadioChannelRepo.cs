using Chess.Model;
using System.Data;
using System.Data.OleDb;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    // TODO: ADD
    public class RadioChannelRepo : IRadioChannelRepository
    {
        public async Task AddChannelAsync(RadioChannelEntity newChannel)
        {
            string sql = "INSERT INTO RadioChannels (ChannelName, ChannelPath, UserID) VALUES (?, ?, ?)";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@ChannelName", newChannel.ChannelName),
                new OleDbParameter("@ChannelPath", newChannel.ChannelPath),
                new OleDbParameter("@UserID", newChannel.UserId)
            );
        }

        public async Task<IEnumerable<RadioChannelEntity>> GetUserChannelsAsync(UserEntity user)
        {
            string sql = "SELECT * FROM RadioChannels WHERE UserID=? OR UserID IS NULL";

            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql, new OleDbParameter("@UserID", user?.Id));

            List<RadioChannelEntity> channels = new List<RadioChannelEntity>();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow channel in dt.Rows)
                {
                    RadioChannelEntity entity = new RadioChannelEntity
                    {
                        Id = (int)channel["ID"],
                        UserId = channel["UserID"]?.ToString() == string.Empty || channel["UserID"] == DBNull.Value ? null : (int)channel["UserID"],
                        ChannelName = channel["ChannelName"].ToString(),
                        ChannelPath = channel["ChannelPath"].ToString(),
                    };
                    
                    channels.Add(entity);
                }

                return channels;
            }

            return null;
        }

        public async Task RemoveChannelAsync(RadioChannelEntity newChannel)
        {
            string sql = "DELETE FROM RadioChannels WHERE ID=?";

            await DbConnectionProvider.ExecuteCommandAsync(sql, new OleDbParameter("@Id", newChannel.Id));
        }
    }
}
