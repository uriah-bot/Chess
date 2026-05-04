using Chess.ViewModel.ViewModelHelper;
using Microsoft.Windows.Input;
using Chess.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Chess.ViewModel
{
    public class AdventureViewModel : ValidatableViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;
        private readonly IGameManagerService _gameManagerService;

        public AdventureViewModel(INavigationService navigationService, IWindowService windowService, IGameManagerService gameManagerService)
        {
            _navigationService = navigationService;
            _windowService = windowService;
            _gameManagerService = gameManagerService;

            _selectedModifiers = new ObservableCollection<ModifierType>();
            //PreviewMouseWheel = new RelayCommand();
            ShowModifierInfoCommand = new RelayCommand(o => ShowModifierInfo());
            StartModifiedGameCommand = new RelayCommand(o => StartModifiedGame(), o => !HasErrors && SelectedModifiers.Count > 0);
            ToggleModifierCommand = new RelayCommand(o => ToggleModifier(o));

            ValidateModifiers();
        }

        public ObservableCollection<ModifierType> SelectedModifiers
        {
            get => _selectedModifiers;
            set
            {
                _selectedModifiers = value;
                OnPropertyChanged();

                ClearErrors();
                ClearErrors(nameof(StartModifiedGameCommand));
                if (SelectedModifiers == null || SelectedModifiers.Count == 0)
                {
                    AddError("Must Choose At Least One Modifier.", nameof(SelectedModifiers));
                }
                if (SelectedModifiers.Contains(ModifierType.Wormholes) && SelectedModifiers.Contains(ModifierType.FogOfWar))
                {
                    AddError("Quantum Chess and Fog of War Cannot Be Selected Together.", nameof(SelectedModifiers));
                }

                OnPropertyChanged(nameof(StartModifiedGameCommand));
            }
        }

        public ICommand ShowModifierInfoCommand { get; }
        public ICommand HideModifierInfoCommand { get; }
        public ICommand StartModifiedGameCommand { get; }
        public ICommand ToggleModifierCommand { get; }

        private ObservableCollection<ModifierType> _selectedModifiers { get; set; }

        private void ToggleModifier(object o)
        {
            var mod = o as string;

            if (Enum.TryParse(mod, out ModifierType modEnum))
            {
                if (SelectedModifiers.Contains(modEnum))
                {
                    SelectedModifiers.Remove(modEnum);
                }
                else
                {
                    SelectedModifiers.Add(modEnum);
                }

                ValidateModifiers();
            }
        }

        private void ValidateModifiers()
        {
            ClearErrors(nameof(SelectedModifiers));
            ClearErrors(nameof(StartModifiedGameCommand));

            if (SelectedModifiers.Count == 0)
            {
                AddError("Must Choose At Least One Modifier.", nameof(SelectedModifiers));
            }
            else if (SelectedModifiers.Contains(ModifierType.Wormholes) && SelectedModifiers.Contains(ModifierType.FogOfWar))
            {
                AddError("\"Quantum Chess\" and \"Fog of War\" Cannot Be Selected Together.", nameof(SelectedModifiers));
            }

            OnPropertyChanged(nameof(SelectedModifiers));
            OnPropertyChanged(nameof(StartModifiedGameCommand));
        }

        private void StartModifiedGame()
        {
            var game = _gameManagerService.ConfigurateGame(SelectedModifiers.ToList());

            _navigationService.NavigateTo<GameViewModel>();
            _windowService.SwitchWindow<MainViewModel>();
        }

        private void ShowModifierInfo()
        {
            throw new NotImplementedException();
        }
    }
}
