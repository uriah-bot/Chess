using Chess.Service;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class LoginViewModel : ValidatableViewModel
    {
        private readonly IAuthService _authService;
        private readonly IUserStore _userStore;
        private readonly IWindowService _windowService;
        private readonly INavigationService _navigationService;

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }
        
        public LoginViewModel(IAuthService authService, IUserStore userStore, IWindowService windowService, INavigationService navigationService)
        {
            _authService = authService;
            _userStore = userStore;
            _windowService = windowService;
            _navigationService = navigationService;
            LoginCommand = new RelayCommand(o => Login(), o => CanUserLogIn());
            NavigateToRegisterCommand = new NavigateCommand<RegisterViewModel>(_navigationService);
        }

        private async void Login()
        {
            (var user, bool exists) = await _authService.LoginAsync(Username, Password);

            if (user != null)
            {
                _userStore.CurrentUser = user;

                _windowService.SwitchWindow<AppBaseViewModel>();
                return;
            }

            if (exists)
            {
                AddError("Password is incorrect.", nameof(LoginCommand));
                return;
            }

            AddError("No user found of this name.", nameof(LoginCommand));
        }

        private bool CanUserLogIn() => !HasErrors && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

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
                ClearErrors(nameof(LoginCommand));
                if (string.IsNullOrWhiteSpace(Username))
                {
                    AddError("Username is a required field.");
                }

                OnPropertyChanged(nameof(LoginCommand));
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
                ClearErrors(nameof(LoginCommand));
                if (string.IsNullOrWhiteSpace(Password))
                {
                    AddError("Password is a required field.");
                }

                OnPropertyChanged(nameof(LoginCommand));
            }
        }

        public override void Dispose()
        {
            Password = string.Empty;

            base.Dispose();
        }
    }
}
