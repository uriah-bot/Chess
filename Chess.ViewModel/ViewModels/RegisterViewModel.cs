using Chess.Model;
using Chess.Service;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class RegisterViewModel : ValidatableViewModel
    {
        private readonly IAuthService _authService;
        private readonly IUserStore _userStore;
        private readonly IWindowService _windowService;
        private readonly INavigationService _navigationService;

        public RegisterViewModel(IAuthService authService, IUserStore userStore, IWindowService windowService, INavigationService navigationService)
        {
            _authService = authService;
            _userStore = userStore;
            _windowService = windowService;
            _navigationService = navigationService;
            RegisterCommand = new RelayCommand(o => Register(), o => CanRegister());
            NavigateToLoginCommand = new NavigateCommand<LoginViewModel>(_navigationService);
            TogglePasswordVisibilityCommand = new RelayCommand(o => TogglePasswordVisibility());
        }

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

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

                ClearErrors();
                ClearErrors(nameof(RegisterCommand));
                if (string.IsNullOrWhiteSpace(Username))
                {
                    AddError("Username is a required field.");
                }

                if (Username.Length < AppConstants.MIN_USERNAME_LENGTH)
                {
                    AddError($"Username is too short ({AppConstants.MIN_USERNAME_LENGTH})");
                }

                if (Username.Length > AppConstants.MAX_USERNAME_LENGTH)
                {
                    AddError($"Username exceeds maximum length ({AppConstants.MAX_USERNAME_LENGTH})");
                }

                OnPropertyChanged(nameof(RegisterCommand));
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

                ClearErrors();
                ClearErrors(nameof(RegisterCommand));
                if (string.IsNullOrWhiteSpace(Password))
                {
                    AddError("Password is a required field.");
                }

                if (Password.Length < AppConstants.MIN_PASSWORD_LENGTH)
                {
                    AddError($"Password is too short ({AppConstants.MIN_PASSWORD_LENGTH})");
                }

                if (Password.Length > AppConstants.MAX_PASSWORD_LENGTH)
                {
                    AddError($"Password exceeds maximum length ({AppConstants.MAX_PASSWORD_LENGTH})");
                }

                OnPropertyChanged(nameof(RegisterCommand));
            }
		}

        public bool IsPasswordVisible { get; set; }

        private async void Register()
        {
            var success = await _authService.RegisterAsync(Username, Password);

            if (success)
            {
                (var user, bool exists) = await _authService.LoginAsync(Username, Password);

                _userStore.CurrentUser = user;

                _windowService.SwitchWindow<AppBaseViewModel>();
                return;
            }

            AddError("A user of this name already exists.", nameof(RegisterCommand));
        }

        private bool CanRegister() => !HasErrors && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        private void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
            OnPropertyChanged(nameof(IsPasswordVisible));
        }

        public override void Dispose()
        {
            Password = string.Empty;

            base.Dispose();
        }
    }
}
