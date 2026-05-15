using Chess.Model;
using Chess.ViewModel.Stores;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class ModifierInfoOverlayViewModel : ViewModelBase, IDialogViewModel
    {
        public Action RequestClose { get; set; }

        private readonly IModifierRepository _modifierRepo;
        private readonly IModifierStore _modifierStore;
        private ModifierData _currentData;

        public ModifierInfoOverlayViewModel(IModifierRepository modifierRepository, IModifierStore modifierStore)
        {
            _modifierRepo = modifierRepository;
            _modifierStore = modifierStore;

            SelectedDynamicItem = _modifierStore.ActivelyInspectedModifier.SelectedParameter;
            Modifier = _modifierStore.ActivelyInspectedModifier.Modifier;
            CloseViewModelCommand = new RelayCommand(o => RequestClose?.Invoke());
        }

		public ModifierType Modifier
		{
			get
			{
				return _modifierStore.ActivelyInspectedModifier.Modifier;
			}
			set
			{
				_modifierStore.ActivelyInspectedModifier.Modifier = value;
                _currentData = _modifierRepo.GetModifierData(Modifier);
                OnPropertyChanged(string.Empty);
			}
		}

        public string SelectedDynamicItem
        {
            get => _modifierStore.ActivelyInspectedModifier.SelectedParameter;
            set
            {
                _modifierStore.ActivelyInspectedModifier.SelectedParameter = value;
            }
        }

        public string ModifierName => _currentData?.Name;
        public string ModifierIconFontFamily => _currentData?.FontFamilyName;
        public string IconFontColor => _currentData?.IconHexColor;
        public bool IsDynamicModifier => _currentData?.IsDynamic ?? false;
        public string ModifierDuration => _currentData?.Duration;
        public List<string> DynamicModifierItemSource => _currentData?.DynamicItems;
        public string ModifierIcon => _currentData?.IconName;
        public string ModifierDescription => _currentData?.Description;
        public string ModifierType => _currentData?.Type;

        public ICommand CloseViewModelCommand { get; }
    }
}
