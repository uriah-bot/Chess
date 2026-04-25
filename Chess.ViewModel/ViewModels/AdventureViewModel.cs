using Chess.ViewModel.ViewModelHelper;
using Microsoft.Windows.Input;
using Chess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class AdventureViewModel : ValidatableViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;
        private readonly GameManagerService _gameManagerService;

        public IPreviewCommand PreviewMouseWheel { get; }
        public ICommand ShowModifierInfoCommand { get; }
        public ICommand HideModifierInfoCommand { get; }
        public ICommand StartModifiedGameCommand { get; }

        private List<ModifierType> SelectedModifiers { get; set; } = new List<ModifierType>();
        public bool HasConflicts => CheckForConflicts();

        public AdventureViewModel(INavigationService navigationService, IWindowService windowService, GameManagerService gameManagerService)
        {
            _navigationService = navigationService;
            _windowService = windowService;
            _gameManagerService = gameManagerService;
            //PreviewMouseWheel = new RelayCommand();
            ShowModifierInfoCommand = new RelayCommand(o => ShowModifierInfo());
            StartModifiedGameCommand = new RelayCommand(o => StartModifiedGame(), o => !CheckForConflicts());
        }

        private void StartModifiedGame()
        {   
            _gameManagerService.Mode = GameMode.Modified;

            var game = _gameManagerService.ConfigurateGame(SelectedModifiers);

            _navigationService.NavigateTo<GameViewModel>();
            _windowService.SwitchWindow<MainViewModel>();
        }

        private void ShowModifierInfo()
        {
            throw new NotImplementedException();
        }

        private bool CheckForConflicts()
        {
            if (SelectedModifiers.Contains(ModifierType.Wormholes) && SelectedModifiers.Contains(ModifierType.FogOfWar))
            {
                AddError("Wormholes cannot be combined with Fog of War.");
                return true;
            }

            return false;
        }

    }
}
