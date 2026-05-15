using Chess.Model;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;

namespace Chess.ViewModel
{
    public partial class GameViewModel : ViewModelBase
    {
		private readonly IUserStore _userStore;
		private readonly IGameManagerService _gameManager;
		private readonly IWindowService _windowService;

        public GameViewModel(IUserStore userStore, IGameManagerService gameManagerService, IWindowService windowService)
		{
			_userStore = userStore;
			_gameManager = gameManagerService;
			_windowService = windowService;

            if (_gameManager.Modifiers.Any(m => m.Modifier == ModifierType.TimeLimit))
			{
				_gameManager.Game.OnModifierDataUpdated += TimersUpdated;
			}

			_gameManager.ConfigurateGame(Enumerable.Empty<ActiveModifier>().ToList());
        }

        // properties for binding to the UI
        public string Username => _userStore.CurrentUser?.Username ?? "Stranger";
		public string AIName => _gameManager.BotRating?.ToString() ?? string.Empty;
		public bool IsClassicalGame => false; /* _gameManager.Mode == GameMode.Classical;*/

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
        private void TimersUpdated(string key, string value)
        {
			switch (key)
			{
				case "WhiteTime":
					WhiteUserTimerText = value;
					break;
				case "BlackTime":
					BlackUserTimerText = value;
					break;

            }
        }

		public PlayerColor CurrentPlayer => _gameManager.Game.CurrentPlayer;

        // methods for interactions (game, user etc.)
        public void OnPromotion(Position from, Position to)
		{
			_windowService.ShowDialog<PromotionMenuViewModel>();
			
        }

		public void RestartGame()
		{

		}

		public async Task OnGameOver()
		{
			await _gameManager.EndGameAsync();

			_windowService.ShowDialog<GameOverMenuViewModel>();
        }


        public override void Dispose()
        {
            if (_gameManager.Modifiers.Any(m => m.Modifier == ModifierType.TimeLimit))
            {
                _gameManager.Game.OnModifierDataUpdated -= TimersUpdated;
            }

            base.Dispose();
        }
	}
}
