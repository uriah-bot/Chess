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
                    Username = userRow["Username"].ToString()!,
                    PasswordHash = userRow["PasswordHash"].ToString()!,
                    PasswordSalt = userRow["PasswordSalt"].ToString()!,
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
    }
}
