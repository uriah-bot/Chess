using Chess.Service;

namespace Chess.ViewModel
{
    public class StatsViewModel : ViewModelBase
    {
		private readonly IUserStore _userStore;
		public StatsViewModel(IUserStore userStore)
		{
			_userStore = userStore;
		}

		public int Wins => _userStore.CurrentUser?.Wins ?? -1;
		public int Draws => _userStore.CurrentUser?.Draws ?? -1;
		public int Losses => _userStore.CurrentUser?.Losses ?? -1;

        public int TotalMatches => Wins == -1 ? -1 : Wins + Draws + Losses;

		public double WinRate => Wins == -1 ? -1 : (double) Wins / TotalMatches;
		public int WinRateBar => (int) WinRate;
		public int Elo => _userStore.CurrentUser?.Elo ?? -1;
		public int PeakElo => _userStore.CurrentUser?.PeakElo ?? -1;
    }
}
