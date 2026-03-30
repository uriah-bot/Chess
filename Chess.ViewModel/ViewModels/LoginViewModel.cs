using Chess.Service;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class LoginViewModel : ViewModelBase
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
            var user = await _authService.LoginAsync(Username, Password);

            if (user != null)
            {
                _userStore.CurrentUser = user;

                _windowService.ShowWindow<AppBaseViewModel>();
                _windowService.CloseCurrentWindow();
            }
        }

        private bool CanUserLogIn()
        {
            return _authService.CanUserLogIn(Username, Password);
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
        // for preventing double clicks when requesting login

        //public LoginViewModel()
        //{
        //    LoginCommand = new RelayCommand(RequestLoginAsync);
        //}
    }
}
