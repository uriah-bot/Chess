using Chess.Model;
using System.Data;
using System.Data.OleDb;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    public class UserRepo : IUserRepository
    {
        public async Task<UserEntity> GetUserByUsernameAsync(string username)
        {
            string sql = "SELECT * FROM Users WHERE Username=?";

            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql, new OleDbParameter("@username", username));

            if (dt.Rows.Count != 0)
            {
                DataRow userRow = dt.Rows[0];
                return new UserEntity
                {
                    Id = (int)userRow["ID"],
                    Username = userRow["Username"].ToString(),
                    PasswordHash = userRow["PasswordHash"].ToString(),
                    PasswordSalt = userRow["PasswordSalt"].ToString(),
                    Elo = (int)userRow["Elo"],
                    PeakElo = (int)userRow["PeakElo"],
                    Wins = (int)userRow["Wins"],
                    Draws = (int)userRow["Draws"],
                    Losses = (int)userRow["Losses"],
                    Role = Enum.TryParse<UserRole>(userRow["Role"]?.ToString(), out var role) ? role : UserRole.User,
                };
            }

            return null;
        }

        public async Task AddUserAsync(UserEntity newUser)
        {
            string sql = "INSERT INTO Users (Username, PasswordHash, PasswordSalt, Elo, PeakElo, Wins, Draws, Losses, Role) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@username", newUser.Username),
                new OleDbParameter("@passwordHash", newUser.PasswordHash),
                new OleDbParameter("@passwordSalt", newUser.PasswordSalt),
                new OleDbParameter("@elo", newUser.Elo),
                new OleDbParameter("@peakElo", newUser.PeakElo),
                new OleDbParameter("@wins", newUser.Wins),
                new OleDbParameter("@draws", newUser.Draws),
                new OleDbParameter("@losses", newUser.Losses),
                new OleDbParameter("@role", newUser.Role.ToString())
            );
        }

        public async Task DeleteUserAsync(UserEntity user)
        {
            string sql = "DELETE FROM Users WHERE ID=?";

            await DbConnectionProvider.ExecuteCommandAsync(sql, new OleDbParameter("@Id", user.Id));
        }

        public async Task<List<LeaderboardEntry>> GetLeaderboardAsync(int count, string property, bool ascending)
        {
            string sql = $"SELECT TOP {count} Username, Elo, Wins FROM Users ORDER BY [{property}] {(ascending ? "ASC" : "DESC")}, ID ASC";
            // wont let me parameterize the column name or order direction, so I have to interpolate them directly into the query string

            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql);

            List<LeaderboardEntry> users = new List<LeaderboardEntry>();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow user in dt.Rows)
                {
                    LeaderboardEntry userEntity = new LeaderboardEntry
                    {
                        Username = user["Username"].ToString(),
                        Elo = (int)user["Elo"],
                        Wins = (int)user["Wins"],
                    };

                    users.Add(userEntity);
                }

                return users;
            }

            return null;
        }

        public async Task UpdateUserAsync(UserEntity user)
        {
            string sql = "UPDATE Users SET Username=?, PasswordHash=?, PasswordSalt=?, Elo=?, PeakElo=?, Wins=?, Draws=?, Losses=?, Role=? WHERE ID=?";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@username", user.Username),
                new OleDbParameter("@passwordHash", user.PasswordHash),
                new OleDbParameter("@passwordSalt", user.PasswordSalt),
                new OleDbParameter("@elo", user.Elo),
                new OleDbParameter("@peakElo", user.PeakElo),
                new OleDbParameter("@wins", user.Wins),
                new OleDbParameter("@draws", user.Draws),
                new OleDbParameter("@losses", user.Losses),
                new OleDbParameter("@role", user.Role.ToString()),
                new OleDbParameter("@id", user.Id)
            );
        }
    }
}
