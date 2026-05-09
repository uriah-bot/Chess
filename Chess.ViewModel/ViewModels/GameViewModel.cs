using Chess.Model;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;

namespace Chess.ViewModel
{
    public partial class GameViewModel : ViewModelBase
    {
		private readonly IUserStore _userStore;
		private readonly IGameManagerService _gameManager;

		public GameViewModel(IUserStore userStore, IGameManagerService gameManagerService)
		{
			_userStore = userStore;
			_gameManager = gameManagerService;
			
        }

        public string Username => _userStore.CurrentUser?.Username ?? "Stranger";
		public string AIName => _gameManager.BotRating?.ToString() ?? string.Empty;
		public bool IsClassicalGame => _gameManager.Mode == GameMode.Classical;

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
