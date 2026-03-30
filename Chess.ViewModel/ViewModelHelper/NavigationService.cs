using Chess.ViewModel.Stores;

namespace Chess.ViewModel.ViewModelHelper
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
    }

    public class NavigationService : INavigationService
    {
        private readonly INavigationStore _navigationStore;
        private readonly Func<Type, ViewModelBase> _viewModelFactory;

        public NavigationService(INavigationStore navigationStore, Func<Type, ViewModelBase> viewModelFactory)
        {
            _navigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            _navigationStore.CurrentViewModel = _viewModelFactory(typeof(TViewModel));
        }
    }
}
