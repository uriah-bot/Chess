namespace Chess.ViewModel.ViewModelHelper
{
    public class NavigateCommand<TViewModel> : RelayCommand where TViewModel : ViewModelBase
    {
        public NavigateCommand(INavigationService navigationService)
            : base(o => navigationService.NavigateTo<TViewModel>()) {}
    }
}
