using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Chess.Model;
using Chess.Data;

namespace Chess.Service
{
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
            var hash = HashPassword(password);

            var user = await _userRepo.GetUserByUsernameAndPasswordAsync(username, hash);

            return user;
        }

        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            var hash = HashPassword(password);

            var copy = await _userRepo.GetUserByUsernameAndPasswordAsync(username, password);

            if (copy != null || copy.Email.Equals(email))
            {
                return false;
            }
            
            var newUser = new UserEntity
            {
                Username = username,
                Email = email,
                PasswordHash = hash
            };

            await _userRepo.AddUserAsync(newUser);

            return true;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(bytes);
            }
        }
    }
}
