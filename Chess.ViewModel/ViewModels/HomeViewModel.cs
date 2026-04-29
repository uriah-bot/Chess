using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUserStore _userStore;

        public HomeViewModel(INavigationService navigationService, IUserStore userStore)
        {
            _navigationService = navigationService;
            _userStore = userStore;
            NavigateToAdventureCommand = new NavigateCommand<AdventureViewModel>(_navigationService);
            NavigateToClassicalCommand = new NavigateCommand<ClassicalViewModel>(_navigationService);
            NavigateToLeaderboardCommand = new NavigateCommand<LeaderboardViewModel>(_navigationService);
        }

        public ICommand NavigateToLeaderboardCommand { get; }
        public ICommand NavigateToAdventureCommand { get; }
        public ICommand NavigateToClassicalCommand { get; }

        public string WelcomeText => $"Welcome back, {_userStore.CurrentUser?.Username ?? "Guest"}";
    }
}
