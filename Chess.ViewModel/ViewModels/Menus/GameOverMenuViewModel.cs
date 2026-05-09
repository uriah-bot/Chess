using Chess.ViewModel.ViewModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class GameOverMenuViewModel : ViewModelBase, IDialogViewModel
    {
        public Action RequestClose { get; set; }

        private readonly INavigationService _navigationService;
        public ICommand ExitCommand { get; }
        public ICommand PlayAgainCommand { get; }

        public GameOverMenuViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ExitCommand = new RelayCommand(o => ExitToApp());
        }

        private void ExitToApp()
        {
            RequestClose?.Invoke();
            new NavigateCommand<AppBaseViewModel>(_navigationService).Execute(null);
        }
    }
}
