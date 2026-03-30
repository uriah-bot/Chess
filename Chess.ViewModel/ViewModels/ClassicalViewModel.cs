using Chess.ViewModel.ViewModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class ClassicalViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;
        public ICommand NavigateToSettingsCommand { get; }
        public ICommand StartClassicalGameCommand { get; }

        public ClassicalViewModel(INavigationService navigationService, IWindowService windowService)
        {
            _navigationService = navigationService;
            _windowService = windowService;
            NavigateToSettingsCommand = new NavigateCommand<SettingsViewModel>(_navigationService);
        }
    }
}
