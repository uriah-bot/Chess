using Chess.ViewModel.Stores;

namespace Chess.ViewModel
{
    public partial class GameViewModel : ViewModelBase
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
				return AIName;
			}
			set
			{
				_AIName = value;
				OnPropertyChanged(nameof(AIName));
			}
		}

		private bool _isClassicalGame = true;
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
    }
}
