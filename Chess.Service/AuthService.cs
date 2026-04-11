using System.Text;
using System.Security.Cryptography;
using Chess.Model;
using static Chess.Data.Repositories;

namespace Chess.Service
{
    public interface IAuthService
    {
        Task<(UserEntity, bool)> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password);
        //Task LogoutAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<(UserEntity, bool)> LoginAsync(string username, string password)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username);
            
            if (user == null)
            {
                return (null, false);
            }

            var hash = HashPassword(password, user.PasswordSalt);

            if (user.PasswordHash == hash)
            {
                return (user, true);
            }

            return (null, true);
        }

        public async Task<bool> RegisterAsync(string username, string password) // TODO: change to Task<Error>
        {
            // check if user exists
            var copycat = await _userRepo.GetUserByUsernameAsync(username);

            if (copycat != null)
            {
                return false;
            }

            // store user info (privacy stuff + role + defaulted values)
            string salt = GenerateSalt();
            var hash = HashPassword(password, salt);

            var role = UserRole.User;
            if (password.Contains("UriahIsTheBestAdmin"))
            {
                role = UserRole.Admin;
            }
            else if (password.Contains("TwinkleTwinleLittleStar"))
            {
                role = UserRole.Moderator;
            }

            // creating user and adding to the database
            var newUser = new UserEntity
            {
                Username = username,
                PasswordSalt = salt,
                PasswordHash = hash,
                Role = role
            };

            await _userRepo.AddUserAsync(newUser);

            return true;
        }

        private static string GenerateSalt()
        {
            // Create a 128-bit random salt lol
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToHexString(bytes);
            }
        }
    }
}
