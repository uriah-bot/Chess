using Chess.Model;

namespace Chess.ViewModel.Stores
{
    public interface IUserStore
    {
        public UserEntity CurrentUser { get; set; }
        public bool IsLoggedIn => CurrentUser != null;

        public event Action CurrentUserChanged;
    }

    public class UserStore : IUserStore
    {
        public event Action CurrentUserChanged;
        private UserEntity _currentUser;
        public UserEntity CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                CurrentUserChanged?.Invoke();
            }
        }

        public void Logout() => CurrentUser = null;

        public void Update(Action<UserEntity> runUpdate)
        {
            if (_currentUser == null) return;

            runUpdate(CurrentUser);

            CurrentUser = CurrentUser; // re-assigned for invoking CurrentUserChanged
        }
    }
}
