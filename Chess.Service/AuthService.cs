using System.Text;
using System.Security.Cryptography;
using Chess.Model;
using Chess.Data;

namespace Chess.Service
{
    public interface IAuthService
    {
        Task<UserEntity> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string email, string password);
        //Task LogoutAsync();
        //Task<bool> ChangeUserPropertyAsync(string username, string password, string propertyName = "");
    }

    public class AuthService : IAuthService
    {
        readonly UserRepo _userRepo = new UserRepo();

        //public async Task<> SendVerificationCode(string Id)
        //{
        //    // Send Email
        //    string body = $"<h1>Your verification code is {code}</h1>";
        //    bool sent = await _emailService.SendEmailAsync(user.Email, "Verification code", body);

        //    return sent ? .Success() : .Failure("Failed to send email. Please check your connection.");
        //}

        public async Task<UserEntity> LoginAsync(string username, string password)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username);
            
            if (user == null)
            {
                return null;
            }

            var hash = HashPassword(password, user.PasswordSalt);

            if (user.PasswordHash == hash)
            {
                return user;
            }

            return null;
        }

        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            var copycat = await _userRepo.GetUserByUsernameAsync(username);

            if (copycat != null || copycat.Email.Equals(email))
            {
                return false;
            }

            string salt = GenerateSalt();
            var hash = HashPassword(password, salt);

            var newUser = new UserEntity
            {
                Username = username,
                Email = email,
                PasswordSalt = salt,
                PasswordHash = hash
            };

            await _userRepo.AddUserAsync(newUser);

            return true;
        }

        private static string GenerateSalt()
        {
            // Create a 128-bit random salt
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
