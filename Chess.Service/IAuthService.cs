using Chess.Model;

namespace Chess.Service
{
    public interface IAuthService
    {
        Task<UserEntity?> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string email, string password);
        //Task LogoutAsync();
        //Task<bool> ChangeUserPropertyAsync(string username, string password, string propertyName = "");
    }
}
