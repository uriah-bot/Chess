using Chess.Model;
using Chess.ViewModel.Stores;

namespace Chess.ViewModel
{
    public partial class GameViewModel : ViewModelBase, IDisposable
    {
		private readonly IUserStore _userStore;
		public GameViewModel(IUserStore userStore)
		{
			_userStore = userStore;

			
        }

        public string Username => _userStore.CurrentUser?.Username ?? "Stranger";

		private string _AIName = string.Empty;
		public string AIName
		{
			get
			{
				return _AIName;
			}
			set
			{
				_AIName = value;
				OnPropertyChanged(nameof(AIName));
			}
		}

		private bool _isClassicalGame;
		public bool IsClassicalGame
		{
			get
			{
				return _isClassicalGame;
			}
			set
			{
				_isClassicalGame = value;
				OnPropertyChanged(nameof(IsClassicalGame));
			}
		}

		private string _whiteUserTimerText = "10:00";
		public string WhiteUserTimerText
		{
			get
			{
				return _whiteUserTimerText;
			}
			set
			{
				_whiteUserTimerText = value;
				OnPropertyChanged(nameof(WhiteUserTimerText));
			}
		}

		private string _blackUserTimerText = "10:00";
		public string BlackUserTimerText
		{
			get
			{
				return _blackUserTimerText;
			}
			set
			{
				_blackUserTimerText = value;
				OnPropertyChanged(nameof(BlackUserTimerText));
			}
		}
	}
}
