using Chess.Model;
using Chess.ViewModel.ViewModelHelper;

namespace Chess.ViewModel.Stores
{
    public interface IUserStore
    {
        public UserEntity CurrentUser { get; set; }
        public bool IsLoggedIn => CurrentUser != null;
        public string AppendingPropertyChange { get; set; }
        public event Action CurrentUserChanged;
        public void Update(Action<UserEntity> runUpdate);
        public void UpdateOnGameEnd(IGameManagerService _gameManager);
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

        public void UpdateOnGameEnd(IGameManagerService _gameManager)
        {
            _gameManager.CalculateEloChange();

            Update(u =>
            {
                if (_gameManager.Mode == GameMode.Modified) return;

                u.Elo += _gameManager.EloDelta.Value;
                if (_gameManager.EloDelta.Value > 0)
                {
                    u.Wins++;
                    if (u.Elo > u.PeakElo)
                    {
                        u.PeakElo = u.Elo;
                    }

                    return;
                }

                if (_gameManager.Game.Result.winner == PlayerColor.None)
                {
                    u.Draws++;
                    return;
                }

                u.Losses++;
            });
        }
    }
}
