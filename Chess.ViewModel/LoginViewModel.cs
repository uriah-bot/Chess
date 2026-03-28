using Chess.Service;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IUserStore _userStore;

        public ICommand LoginCommand { get; }
        
        public LoginViewModel(IAuthService authService, IUserStore userStore)
        {
            _authService = authService;
            _userStore = userStore;
            LoginCommand = new RelayCommand(o => Login(), o => CanUserLogIn());
        }

        private async void Login()
        {
            var user = await _authService.LoginAsync(Username, Password);

            if (user != null)
            {
                _userStore.CurrentUser = user;
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
