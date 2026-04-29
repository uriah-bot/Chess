using Chess.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Chess.Data.Repositories;

namespace Chess.ViewModel
{
    public class LeaderboardViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;
        public ICommand SortCommand { get; private set; }

        public List<int> PlayerCountOptions { get; } = new List<int> { 10, 50, 100, 200, 500 };
        private ObservableCollection<UserEntity> _users;
        public ObservableCollection<UserEntity> Users
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

        public LeaderboardViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            return IsAscending ? " ▲" : " ▼";
        }

        private async Task LoadUsersAsync()
        {
            var users = await _userRepository.GetLeaderboardAsync(PlayerCount, SortBy, IsAscending);
            Users = new ObservableCollection<UserEntity>(users);
        }
    }
}
