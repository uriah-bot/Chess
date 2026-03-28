using Chess.Service;

namespace Chess.ViewModel
{
    public class AppBaseSidebarViewModel : ViewModelBase, IDisposable
    {
        private readonly IUserStore _userStore;
        public AppBaseSidebarViewModel(IUserStore userStore)
		{
			_userStore = userStore;
			_userStore.CurrentUserChanged += OnUserChanged;
		}

		public string Username => _userStore.CurrentUser?.Username ?? "Stranger"; 

		public int Elo => _userStore.CurrentUser?.Elo ?? -1;

        public void Dispose() // might stay in memory even when in a different view
        {
            _userStore.CurrentUserChanged -= OnUserChanged;
        }

        private void OnUserChanged()
		{
			OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Elo));
        }
	}
}
