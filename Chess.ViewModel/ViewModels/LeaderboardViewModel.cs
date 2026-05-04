using Chess.Model;
using Chess.ViewModel.Stores;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Chess.Data.Repositories;

namespace Chess.ViewModel
{
    public class LeaderboardViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserStore _userStore;
        public ICommand SortCommand { get; private set; }

        public List<int> PlayerCountOptions { get; } = new List<int> { 10, 50, 100, 200, 500 };
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
        public bool IsAscending { get; set; }
        public string SortBy { get; set; } = "Elo";
        private int _playerCount = 10;
        public int PlayerCount
        {
            get => _playerCount;
            set
            {
                _playerCount = value;
                OnPropertyChanged(nameof(PlayerCount));
                _ = LoadUsersAsync();
            }
        }

        public string UsernameSortIcon => GetSortIcon("Username");
        public string EloSortIcon => GetSortIcon("Elo");
        public string WinsSortIcon => GetSortIcon("Wins");

        public LeaderboardViewModel(IUserRepository userRepository, IUserStore userStore)
        {
            _userRepository = userRepository;
            _userStore = userStore;

            SortCommand = new RelayCommand(o => ExecuteSort(o));
            _ = LoadUsersAsync();
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

            _ = LoadUsersAsync();
        }

        private string GetSortIcon(string column)
        {
            if (SortBy != column) return ""; // No arrow if not sorted by this
            return IsAscending ? " ▲" : " ▼";        }

        private async Task LoadUsersAsync()
        {
            var users = await _userRepository.GetLeaderboardAsync(PlayerCount, SortBy, IsAscending);

            int index = users.FindIndex(u => u.Username == _userStore.CurrentUser?.Username);

            if (index != -1) // -1 means not found
            {
                var entry = users[index];
                entry.IsCurrentUser = true;
                users[index] = entry;
            }

            //if (!users.Any(u => u.Username == _userStore.CurrentUser.Username))
            //{
            //    users.RemoveAt(users.Count - 1);
            //    users.Add(_userStore.CurrentUser);
            //}


            Users = new ObservableCollection<LeaderboardEntry>(users);
        }
    }
}
