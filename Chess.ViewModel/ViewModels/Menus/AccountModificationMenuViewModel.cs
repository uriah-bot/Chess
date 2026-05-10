using Chess.Model;
using Chess.Service;
using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Chess.Data.Repositories;

namespace Chess.ViewModel
{
    public class AccountModificationMenuViewModel : ValidatableViewModel, IDialogViewModel
    {
        public Action RequestClose { get; set; }

        private readonly IUserStore _userStore;
        private readonly IUserRepository _userRepo;
        private readonly IAuthService _authService;

        public AccountModificationMenuViewModel(IUserStore userStore, IUserRepository userRepository, IAuthService authService)
        {
            _userStore = userStore;
            _userRepo = userRepository;
            _authService = authService;

            ChangeUsernameCommand = new RelayCommand(o => ChangeUsername(), o => !HasErrors && !string.IsNullOrWhiteSpace(NewUsername));
            ChangePasswordCommand = new RelayCommand(o => ChangePasswordAsync(), o => !HasErrors && !string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(Password));
            RequestRoleCommand = new RelayCommand(o => RequestRole(o));
            NavigateBackCommand = new RelayCommand(o => RequestClose?.Invoke());
        }

        private string _newUsername;
        public string NewUsername
        {
            get
            {
                return _newUsername;
            }
            set
            {
                _newUsername = value;
                OnPropertyChanged(nameof(NewUsername));

                ClearErrors();
                ClearErrors(nameof(ChangeUsernameCommand));

                if (string.IsNullOrWhiteSpace(NewUsername))
                {
                    AddError("Username is a required field.");
                }

                if (NewUsername.Length < AppConstants.MIN_USERNAME_LENGTH)
                {
                    AddError($"Username is too short ({AppConstants.MIN_USERNAME_LENGTH})");
                }

                if (NewUsername.Length > AppConstants.MAX_USERNAME_LENGTH)
                {
                    AddError($"Username exceeds maximum length ({AppConstants.MAX_USERNAME_LENGTH})");
                }

                OnPropertyChanged(nameof(ChangeUsernameCommand));
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
                ClearErrors(nameof(ChangePasswordCommand));
                if (string.IsNullOrWhiteSpace(Password))
                {
                    AddError("Confirmation Password is a required field.");
                }

                OnPropertyChanged(nameof(ChangePasswordCommand));
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));

                ClearErrors();
                ClearErrors(nameof(ChangePasswordCommand));
                if (string.IsNullOrWhiteSpace(NewPassword))
                {
                    AddError("New Password is a required field.");
                }

                if (NewPassword.Length < AppConstants.MIN_PASSWORD_LENGTH)
                {
                    AddError($"Password is too short ({AppConstants.MIN_PASSWORD_LENGTH})");
                }

                if (NewPassword.Length > AppConstants.MAX_PASSWORD_LENGTH)
                {
                    AddError($"Password exceeds maximum length ({AppConstants.MAX_PASSWORD_LENGTH})");
                }

                OnPropertyChanged(nameof(ChangePasswordCommand));
            }
        }

        public string PopupMode => _userStore.AppendingPropertyChange;

        public ICommand ChangeUsernameCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand RequestRoleCommand { get; }
        public ICommand NavigateBackCommand { get; }

        private void RequestRole(object o)
        {
            throw new NotImplementedException();
        }

        private async void ChangeUsername()
        {
            var existingUser = await _userRepo.GetUserByUsernameAsync(NewUsername);

            if (existingUser != null)
            {
                AddError("User of this name already exists", nameof(ChangeUsernameCommand));
                return;
            }

            _userStore.Update(user => user.Username = NewUsername);
            await _userRepo.UpdateUserAsync(_userStore.CurrentUser);

            RequestClose?.Invoke();
        }

        private async void ChangePasswordAsync()
        {
            (UserEntity successPassword, bool userExists) = await _authService.LoginAsync(_userStore.CurrentUser.Username, Password);

            if (successPassword == null)
            {
                AddError("Incorrect Confirmation Password", nameof(ChangePasswordCommand));
                OnPropertyChanged(nameof(ChangePasswordCommand));
                return;
            }

            _userStore.Update(u => u.PasswordHash = AuthService.HashPassword(NewPassword, _userStore.CurrentUser.PasswordSalt));
            await _userRepo.UpdateUserAsync(_userStore.CurrentUser);
            RequestClose?.Invoke();
        }
    }
}
