using Chess.Model;
using System.Data;
using System.Data.OleDb;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    public class SettingsRepo : ISettingsRepository
    {
        public async Task<SettingsModel> GetUserSettingAsync(UserEntity user)
        {
            string sql = "SELECT * FROM Settings WHERE UserID=?";

            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql, new OleDbParameter("@username", user.Id));

            if (dt.Rows.Count != 0)
            {
                DataRow settings = dt.Rows[0];
                return new SettingsModel
                {
                    UserId = user.Id,
                    CurrentSong = settings["CurrentSong"].ToString(),
                    DisplayCoordinates = (bool)settings["DisplayCoordinates"],
                    Volume = (double)settings["Volume"],
                    StopRadioOnMatches = (bool)settings["StopRadioOnMatches"],
                };
            }

            return null;
        }

        public async Task UpdateUserSettingsAsync(UserEntity user)
        {
            string sql = "UPDATE Settings SET Volume=?, CurrentSong=?, StopRadioOnMatches=?, DisplayCoordinates=? WHERE UserID=?";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@volume", user.Settings.Volume),
                new OleDbParameter("@currentSong", user.Settings.CurrentSong),
                new OleDbParameter("@stopRadioOnMatches", user.Settings.StopRadioOnMatches),
                new OleDbParameter("@displayCoordinates", user.Settings.DisplayCoordinates),
                new OleDbParameter("@id", user.Id)
            );
        }

        public async Task AddUserSettingsAsync(UserEntity user)
        {
            string sql = "INSERT INTO Settings (UserID) VALUES (?)";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@id", user.Id)
            );
        }
    }
}
