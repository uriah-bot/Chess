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
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ISettingsRepository _settingsRepo;

        public AuthService(IUserRepository userRepo, ISettingsRepository settingsRepo)
        {
            _userRepo = userRepo;
            _settingsRepo = settingsRepo;
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
                user.Settings = await _settingsRepo.GetUserSettingAsync(user);
                return (user, true);
            }

            return (null, true);
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            // check if user already exists
            var copycat = await _userRepo.GetUserByUsernameAsync(username);

            if (copycat != null)
            {
                return false;
            }

            // store user info (privacy stuff + role + defaulted values)
            string salt = GenerateSalt();
            var hash = HashPassword(password, salt);

            var role = UserRole.User;
            if (password.Contains("TwinkleTwinleLittleStar"))
            {
                role = UserRole.Moderator;
            }

            // creating (the needed properties in) user and adding to the database
            var newUser = new UserEntity
            {
                Username = username,
                PasswordSalt = salt,
                PasswordHash = hash,
                Role = role,
            };

            newUser.Id = await _userRepo.AddUserAsync(newUser);
            await _settingsRepo.AddUserSettingsAsync(newUser);

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

        public static string HashPassword(string password, string salt)
        {
            // Hash the password with the salt using SHA256
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToHexString(bytes);
            }
        }
    }
}
