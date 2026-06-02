using Chess.Model;
using Chess.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Chess.Data.Repositories;

namespace Chess.ViewModel
{
    public class AdvancedSettingsViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserStore _userStore;

        public AdvancedSettingsViewModel(IUserRepository userRepository, IUserStore userStore)
        {
            _userRepository = userRepository;
            _userStore = userStore;

            PromoteUserCommand = new RelayCommand(async o => await PromoteUser(o), o => CanPromoteUser(o));
            DemoteUserCommand = new RelayCommand(async o => await DemoteUserAsync(o), o => CanDemoteUser(o));
            DeleteUserCommand = new RelayCommand(async o => await DeleteUser(o), o => CanDeleteUser(o));
            SortCommand = new RelayCommand(o => ExecuteSort(o));
            _ = ReloadUsersAsync();
        }

        public ICommand PromoteUserCommand { get; }
        public ICommand DemoteUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand SortCommand { get; }

        public bool IsAscending { get; set; }
        public string SortBy { get; set; } = "Role";

        public string UsernameSortIcon => GetSortIcon("Username");
        public string EloSortIcon => GetSortIcon("Elo");
        public string WinsSortIcon => GetSortIcon("Wins");
        public string RoleSortIcon => GetSortIcon("Role");

        private ObservableCollection<LeaderboardEntry> _users;
        public ObservableCollection<LeaderboardEntry> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private int _playerCount = 10;
        public int PlayerCount
        {
            get => _playerCount;
            set
            {
                _playerCount = value;
                OnPropertyChanged(nameof(PlayerCount));
            }
        }

        private async Task ReloadUsersAsync()
        {
            var users = await _userRepository.GetLeaderboardAsync(-1, SortBy, IsAscending);

            int index = users.FindIndex(u => u.Username == _userStore.CurrentUser?.Username);

            if (index != -1) // -1 means not found
            {
                users.RemoveAt(index);
            }

            Users = new ObservableCollection<LeaderboardEntry>(users);
        }

        private void ExecuteSort(object parameter)
        {
            string column = parameter as string;

            if (SortBy == column)
            {
                IsAscending = !IsAscending;
            }
            else
            {
                SortBy = column;
                IsAscending = false;
            }

            OnPropertyChanged(nameof(UsernameSortIcon));
            OnPropertyChanged(nameof(EloSortIcon));
            OnPropertyChanged(nameof(WinsSortIcon));
            OnPropertyChanged(nameof(RoleSortIcon));

            _ = ReloadUsersAsync();
        }

        private string GetSortIcon(string column)
        {
            if (SortBy != column) return ""; // no arrow if not sorted by this

            return IsAscending ? " ▲" : " ▼";
        }

        private async Task PromoteUser(object o)
        {
            var user = new UserEntity((LeaderboardEntry)o);
            user.Role++;
            await _userRepository.UpdateUserAsync(user, "Role", user.Role);

            _ = ReloadUsersAsync();
        }

        private static bool CanPromoteUser(object o)
        {
            var user = new UserEntity((LeaderboardEntry)o);
            return user.Role < UserRole.Moderator;
        }

        private async Task DemoteUserAsync(object o)
        {
            var user = new UserEntity((LeaderboardEntry)o);
            user.Role--;
            await _userRepository.UpdateUserAsync(user, "Role", user.Role);

            _ = ReloadUsersAsync();
        }

        private static bool CanDemoteUser(object o)
        {
            var user = new UserEntity((LeaderboardEntry)o);
            return user.Role > UserRole.User && user.Role < UserRole.Admin;
        }

        private async Task DeleteUser(object o)
        {
            var user = new UserEntity((LeaderboardEntry)o);
            await _userRepository.DeleteUserAsync(user, true);

            _ = ReloadUsersAsync();
        }

        private static bool CanDeleteUser(object o)
        {
            var user = new UserEntity((LeaderboardEntry)o);
            return user.Role == UserRole.User || user.Role == UserRole.Moderator;
        }
    }
}