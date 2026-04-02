using Chess.ViewModel.ViewModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToAdventureCommand = new NavigateCommand<AdventureViewModel>(_navigationService);
            NavigateToClassicalCommand = new NavigateCommand<ClassicalViewModel>(_navigationService);
        }
        public ICommand NavigateToAdventureCommand { get; }
        public ICommand NavigateToClassicalCommand { get; }
    }
}
