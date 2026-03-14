using Chess.Model;
using System.Data;
using System.Data.OleDb;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    public class UserRepo : IUserRepository
    {

        public async Task<UserEntity> GetUserByUsernameAndPasswordAsync(string username, string HashedPassword)
        {
            string sql = "SELECT Id, Email ,Elo, Role FROM Users WHERE Username=? AND PasswordHash=?";

            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql, new OleDbParameter(@username, "username"), new OleDbParameter(@HashedPassword, "passwordHash"));

            if (dt.Rows.Count != 0)
            {
                DataRow userRow = dt.Rows[0];
                return new UserEntity
                {
                    Id = (int)userRow["Id"],
                    Username = userRow["Username"].ToString()!,
                    PasswordHash = userRow["PasswordHash"].ToString()!,
                    PasswordSalt = userRow["PasswordSalt"].ToString()!,
                    Email = userRow["Email"].ToString()!,
                    Elo = (int)userRow["Elo"],
                    Role = (UserRole)userRow["Role"]
                };
            }

            return null;
        }

        public async Task AddUserAsync(UserEntity newUser)
        {
            string sql = "INSERT INTO Users (Username, Password, Email, Elo, Role) VALUES (?, ?, ?, ?, ?)";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@username", newUser.Username),
                new OleDbParameter("@password", newUser.PasswordHash),
                new OleDbParameter("@email", newUser.Email),
                new OleDbParameter("@elo", newUser.Elo),
                new OleDbParameter("@role", newUser.Role.ToString())
            );
        }

        public async Task UpdateRoleAsync(string email, UserRole newRole)
        {
            string sql = "UPDATE Users SET Role = ? WHERE Email = ?";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@role", (int)newRole),
                new OleDbParameter("@email", email)
            );
        }
    }
}
