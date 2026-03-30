using Chess.ViewModel.ViewModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class AdventureViewModel : ViewModelBase
    {
        private readonly IWindowService _windowService;
        public ICommand ShowModifierInfoCommand { get; }
        public ICommand HideModifierInfoCommand { get; }
        public ICommand StartModifiedGameCommand { get; }
        public AdventureViewModel(IWindowService windowService)
        {
            _windowService = windowService;
        }
    }
}
