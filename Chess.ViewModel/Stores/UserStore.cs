using Chess.Model;

namespace Chess.ViewModel.Stores
{
    public interface IUserStore
    {
        public UserEntity CurrentUser { get; set; }
        public bool IsLoggedIn => CurrentUser != null;
        public string AppendingPropertyChange { get; set; }
        public event Action CurrentUserChanged;
        public void Update(Action<UserEntity> runUpdate);
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

        public string AppendingPropertyChange { get; set; }

        public void Logout() => CurrentUser = null;

        public void Update(Action<UserEntity> runUpdate)
        {
            if (_currentUser == null) return;

            runUpdate(CurrentUser);

            CurrentUserChanged?.Invoke(); // re-assigned for invoking CurrentUserChanged
        }
    }
}
