using Chess.ViewModel.Stores;

namespace Chess.ViewModel
{
    public class StatsViewModel : ViewModelBase
    {
        private readonly IUserStore _userStore;
        public StatsViewModel(IUserStore userStore)
        {
            _userStore = userStore;
            _userStore.CurrentUserChanged += OnUserChanged;
        }

        public int Wins => _userStore.CurrentUser?.Wins ?? -1;
        public int Draws => _userStore.CurrentUser?.Draws ?? -1;
        public int Losses => _userStore.CurrentUser?.Losses ?? -1;

        public int TotalMatches => Wins == -1 ? -1 : Wins + Draws + Losses;

        public double WinRate
        {
            get
            {
                if (Wins == -1)
                {
                    return -1;
                }
                else if (TotalMatches == 0)
                {
                    return 0;
                }
                
                return (double)Wins / TotalMatches;
            }
        }
		public int WinRateBar => (int) WinRate;
		public int Elo => _userStore.CurrentUser?.Elo ?? -1;
		public int PeakElo => _userStore.CurrentUser?.PeakElo ?? -1;

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(Wins));
            OnPropertyChanged(nameof(Draws));
            OnPropertyChanged(nameof(Losses));
            OnPropertyChanged(nameof(TotalMatches));
            OnPropertyChanged(nameof(WinRate));
            OnPropertyChanged(nameof(WinRateBar));
            OnPropertyChanged(nameof(Elo));
            OnPropertyChanged(nameof(PeakElo));
        }
        // TODO: How will it know when user is changed
    }
}
