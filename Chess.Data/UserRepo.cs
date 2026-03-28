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
            string sql = "SELECT Id, Username, PasswordHash, PasswordSalt ,Elo, Role FROM Users WHERE Username=?";

            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql, new OleDbParameter(@username, "username"));

            if (dt.Rows.Count != 0)
            {
                DataRow userRow = dt.Rows[0];
                return new UserEntity
                {
                    Id = (int)userRow["Id"],
                    Username = userRow["Username"].ToString()!,
                    PasswordHash = userRow["PasswordHash"].ToString()!,
                    PasswordSalt = userRow["PasswordSalt"].ToString()!,
                    Elo = (int)userRow["Elo"],
                    Role = (UserRole)userRow["Role"]
                };
            }

            return null;
        }

        public async Task AddUserAsync(UserEntity newUser)
        {
            string sql = "INSERT INTO Users (Username, PasswordHash, PasswordSalt, Role) VALUES (?, ?, ?, ?, ?, ?)";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@username", newUser.Username),
                new OleDbParameter("@passwordHash", newUser.PasswordHash),
                new OleDbParameter("@elo", newUser.Elo),
                new OleDbParameter("@role", newUser.Role.ToString())
            );
        }

        public async Task UpdateRoleAsync(string username, UserRole newRole)
        {
            string sql = "UPDATE Users SET Role = ? WHERE Username = ?";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@role", newRole.ToString()),
                new OleDbParameter("@username", username)
            );
        }

        public async Task UpdateEloAsync(string username, int updatedElo)
        {
            string sql = "UPDATE Users SET Role = ? WHERE Username = ?";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@elo", updatedElo),
                new OleDbParameter("@username", username)
            );
        }
    }
}
