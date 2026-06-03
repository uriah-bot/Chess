using Chess.ViewModel.ViewModelHelper;
using Chess.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Chess.ViewModel.Stores;

namespace Chess.ViewModel
{
    public class AdventureViewModel : ValidatableViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;
        private readonly IGameManagerService _gameManagerService;
        private readonly IModifierStore _modifierStore;
        private readonly IUserStore _userStore;

        private readonly Dictionary<ModifierType, ActiveModifier> _modifierStates = new Dictionary<ModifierType, ActiveModifier>();

        public AdventureViewModel(INavigationService navigationService, IWindowService windowService, IGameManagerService gameManagerService, IModifierStore modifierStore, IUserStore userStore)
        {
            _navigationService = navigationService;
            _windowService = windowService;
            _gameManagerService = gameManagerService;
            _modifierStore = modifierStore;
            _userStore = userStore;

            ShowModifierInfoCommand = new RelayCommand(o => ShowModifierInfo(o));
            StartModifiedGameCommand = new RelayCommand(o => StartModifiedGame(), o => !HasErrors && SelectedModifiers.Count > 0 && _userStore.IsLoggedIn);
            ToggleModifierCommand = new RelayCommand(o => ToggleModifier(o));

            ValidateModifiers();
        }

        public ObservableCollection<ActiveModifier> SelectedModifiers { get; } = new ObservableCollection<ActiveModifier>();

        public int ModifierCount => SelectedModifiers.Count;

        public ICommand ShowModifierInfoCommand { get; }
        public ICommand StartModifiedGameCommand { get; }
        public ICommand ToggleModifierCommand { get; }

        private ActiveModifier GetModifierState(ModifierType mod)
        {
            if (!_modifierStates.ContainsKey(mod))
            {
                _modifierStates[mod] = new ActiveModifier
                {
                    Modifier = mod,
                    SelectedParameter = mod switch
                    {
                        ModifierType.Poof => AppConstants.POOF_DEFAULT_MOVES.ToString(),
                        ModifierType.TimeLimit => AppConstants.TIME_LIMIT_DEFAULT_TIME.ToString(),
                        ModifierType.MoveMultiplier => AppConstants.MOVE_MULTIPLIER_DEFAULT_MULTIPLIER.ToString(),
                        ModifierType.Wormholes => AppConstants.WORMHOLES_DEFAULT_PORTALS.ToString(),
                        _ => null,
                    }
                };
            }
            return _modifierStates[mod];
        }

        private void ToggleModifier(object o)
        {
            var mod = o as string;

            if (Enum.TryParse(mod, out ModifierType modEnum))
            {
                var toRemove = SelectedModifiers.FirstOrDefault(m => m.Modifier == modEnum);

                if (toRemove != null)
                {
                    SelectedModifiers.Remove(toRemove);
                }
                else
                {
                    SelectedModifiers.Add(GetModifierState(modEnum));
                }

                OnPropertyChanged(nameof(ModifierCount));
                ValidateModifiers();
            }
        }

        private void ValidateModifiers()
        {
            ClearErrors(nameof(SelectedModifiers));
            ClearErrors(nameof(StartModifiedGameCommand));

            if (SelectedModifiers == null || SelectedModifiers.Count == 0)
            {
                AddError("Must Choose At Least One Modifier.", nameof(SelectedModifiers));
            }
            if (SelectedModifiers.Any(m => m.Modifier == ModifierType.Wormholes) && SelectedModifiers.Any(m => m.Modifier == ModifierType.FogOfWar))
            {
                AddError("Quantum Chess and Fog of War Cannot Be Selected Together.", nameof(SelectedModifiers));
            }
            if (SelectedModifiers.Any(m => m.Modifier == ModifierType.MoveMultiplier) && SelectedModifiers.Any(m => m.Modifier == ModifierType.Poof))
            {
                AddError("Move Multiplier and Poof Cannot Be Selected Together.", nameof(SelectedModifiers));
            }

            OnPropertyChanged(nameof(StartModifiedGameCommand));
        }

        private void StartModifiedGame()
        {
            _gameManagerService.Modifiers = SelectedModifiers.ToList();
            _gameManagerService.Mode = GameMode.Modified;

            _navigationService.NavigateTo<GameViewModel>();
            _windowService.SwitchWindow<MainViewModel>();
        }

        private void ShowModifierInfo(object parameter)
        {
            var modStr = parameter as string;

            if(Enum.TryParse(modStr, out ModifierType mod))
            {
                _modifierStore.ActivelyInspectedModifier = GetModifierState(mod);

                _windowService.ShowDialog<ModifierInfoOverlayViewModel>();
            }
        }
    }
}
