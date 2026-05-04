//using Chess.Model;
//using System.Data;
//using System.Data.OleDb;
//using static Chess.Data.Repositories;

//namespace Chess.Data
//{
//    public class SettingsRepo : ISettingsRepository
//    {
//        public async Task<SettingsModel> GetUserSetting(UserEntity user)
//        {
//            string sql = "SELECT * FROM Settings WHERE UserID=?";

//            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql, new OleDbParameter("@username", user.Id));

//            if (dt.Rows.Count != 0)
//            {
//                DataRow settings = dt.Rows[0];
//                return new SettingsModel
//                {
//                    DisplayCoordinates = (bool)settings["DisplayCoordinates"],
//                    SoundEffectOnMove = (bool)settings["SoundEffectOnMove"],
//                    Volume = (double)settings["Volume"],
//                    StopRadioOnMatches = (bool)settings["StopRadioOnMatches"],
//                };
//            }

//            return null;
//        }

//        public async Task UpdateUserSettings(UserEntity user)
//        {
//            string sql = "UPDATE Settings SET Volume=?, StopRadioOnMatches=?, SoundEffectOnMove=?, DisplayCoordinates=? WHERE UserID=?";

//            await DbConnectionProvider.ExecuteCommandAsync(sql,
//                new OleDbParameter("@volumn", user.Settings.Volume),
//                new OleDbParameter("@stopRadioOnMatches", user.Settings.StopRadioOnMatches),
//                new OleDbParameter("@soundEffectOnMove", user.Settings.SoundEffectOnMove),
//                new OleDbParameter("@displayCoordinates", user.Settings.DisplayCoordinates),
//                new OleDbParameter("@id", user.Id)
//            );
//        }
//    }
//}
