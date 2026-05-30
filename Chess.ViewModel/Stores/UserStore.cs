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

            if (_gameManager.Mode == GameMode.Modified) return;

            Update(u =>
            {
                if (_gameManager.EloDelta.Value < 0)
                {
                    u.Losses++;
                }
                else if (_gameManager.Game.Result.winner == PlayerColor.None)
                {
                    u.Draws++;
                }
                else
                {
                    u.Wins++;
                    if (u.Elo > u.PeakElo)
                    {
                        u.PeakElo = u.Elo;
                    }
                }

                if (_gameManager.EloDelta.Value < 0 && u.Elo < 300 - _gameManager.EloDelta.Value)
                {
                    u.Elo = 300;
                    // limit the elo to 300
                    return;
                }

                if (_gameManager.EloDelta.Value > 0 && u.Elo > 3200 - _gameManager.EloDelta.Value)
                {
                    u.Elo = 3200;
                    // limit the elo to 3200
                    return;
                }

                u.Elo += _gameManager.EloDelta.Value;
            });
        }
    }
}
