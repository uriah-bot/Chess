using Chess.Service;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IUserStore _userStore;
        private readonly IWindowService _windowService;
        private readonly INavigationService _navigationService;

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public RegisterViewModel(IAuthService authService, IUserStore userStore, IWindowService windowService, INavigationService navigationService)
        {
            _authService = authService;
            _userStore = userStore;
            _windowService = windowService;
            _navigationService = navigationService;
            RegisterCommand = new RelayCommand(o => Register(), o => CanRegister());
            NavigateToLoginCommand = new NavigateCommand<LoginViewModel>(_navigationService);
        }

        private async void Register()
        {
            var success = await _authService.RegisterAsync(Username, Password);

            if (success)
            {
                var user = await _authService.LoginAsync(Username, Password);

                _userStore.CurrentUser = user;

                _windowService.ShowWindow<AppBaseViewModel>();
                _windowService.CloseCurrentWindow();
            }
        }

        private bool CanRegister()
        {
            return _authService.CanUserRegister(Username, Password);
        }

        private string _username;
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}

		private string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

        private bool _isPasswordVisible = false;
        public bool IsPasswordVisible
        {
            get
            {
                return _isPasswordVisible;
            }
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged(nameof(IsPasswordVisible));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        //private bool _isLoading;
        //public bool IsLoading
        //{
        //    get => _isLoading;
        //    set
        //    {
        //        _isLoading = value;
        //        OnPropertyChanged(nameof(IsLoading));
        //    }
        //}
        // for preventing double clicks when requesting registration and nice UI

        //public RegisterViewModel()
        //{
        //    RegisterCommand = new RelayCommand(RequestRegistrationAsync);
        //}
    }
}
