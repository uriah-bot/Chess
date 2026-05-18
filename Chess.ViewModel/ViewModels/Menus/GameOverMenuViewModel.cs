using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class GameOverMenuViewModel : ViewModelBase, IDialogViewModel
    {
        public Action RequestClose { get; set; }

        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;

        public ICommand ExitCommand { get; }
        public ICommand PlayAgainCommand { get; }

        public GameOverMenuViewModel(INavigationService navigationService, IWindowService windowService)
        {
            _navigationService = navigationService;
            _windowService = windowService;

            ExitCommand = new RelayCommand(o => ExitToApp());
            PlayAgainCommand = new RelayCommand(o => PlayAgain());
        }

        private void PlayAgain()
        {
            RequestClose?.Invoke();

            _navigationService.NavigateTo<GameViewModel>();
        }

        private void ExitToApp()
        {
            RequestClose?.Invoke();
            
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _windowService.SwitchWindow<AppBaseViewModel>();
            }), System.Windows.Threading.DispatcherPriority.Background); // <-- THIS IS THE MAGIC
        }
    }
}
