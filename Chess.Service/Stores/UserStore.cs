using Chess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Service
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

        public void Update(Action<UserEntity> runUpdate)
        {
            if (_currentUser != null) return;

            runUpdate(CurrentUser);

            CurrentUser = CurrentUser; // re-assigned for invoking CurrentUserChanged
        }
    }
}
