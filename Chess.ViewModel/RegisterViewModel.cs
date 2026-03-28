using Chess.Service;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IUserStore _userStore;

        public ICommand RegisterCommand;

        public RegisterViewModel(IAuthService authService, IUserStore userStore)
        {
            _authService = authService;
            _userStore = userStore;
            RegisterCommand = new RelayCommand(o => Register(), o => CanRegister());
        }

        private async void Register()
        {
            var success = await _authService.RegisterAsync(Username, Password);

            if (success)
            {
                var user = await _authService.LoginAsync(Username, Password);
                _userStore.CurrentUser = user;
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
