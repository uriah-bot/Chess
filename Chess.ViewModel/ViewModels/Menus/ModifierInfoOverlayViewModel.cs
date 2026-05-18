using Chess.Data;
using Chess.Model;
using Chess.ViewModel.Stores;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class ModifierInfoOverlayViewModel : ViewModelBase, IDialogViewModel
    {
        public Action RequestClose { get; set; }

        private readonly IJSONRepository<string, ModifierData> _jsonRepo;
        private readonly IModifierStore _modifierStore;
        private ModifierData _currentData;

        public ModifierInfoOverlayViewModel(IJSONRepository<string, ModifierData> jsonRepository, IModifierStore modifierStore)
        {
            _jsonRepo = jsonRepository;
            _modifierStore = modifierStore;

            CloseViewModelCommand = new RelayCommand(o => RequestClose?.Invoke());

            CurrentDataChangedAsync(Modifier.ToString());
        }

        public ICommand CloseViewModelCommand { get; }

        public ModifierType Modifier
		{
			get
			{
				return _modifierStore.ActivelyInspectedModifier.Modifier;
			}
			set
			{
				_modifierStore.ActivelyInspectedModifier.Modifier = value;
                CurrentDataChangedAsync(value.ToString());
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

        public string ModifierName => _currentData?.Name ?? "Loading...";
        public string ModifierIconFontFamily => _currentData?.FontFamilyName;
        public string IconFontColor => _currentData?.IconHexColor;
        public bool IsDynamicModifier => _currentData?.IsDynamic ?? false;
        public string ModifierDuration => _currentData?.Duration ?? "Loading...";
        public List<string> DynamicModifierItemSource => _currentData?.DynamicItems;
        public string ModifierIcon => _currentData?.IconName ?? "Loading...";
        public string ModifierDescription => _currentData?.Description ?? "Loading...";
        public string ModifierType => _currentData?.Type ?? "Loading...";

        private async void CurrentDataChangedAsync(string value)
        {
            _currentData = await _jsonRepo.FetchFromJSONAsync("Modifiers.json", value);
            OnPropertyChanged(string.Empty);
        }
    }
}
