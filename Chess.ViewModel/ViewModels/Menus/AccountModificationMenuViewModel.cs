using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Chess.Data.Repositories;

namespace Chess.ViewModel
{
    public class AccountModificationMenuViewModel : DialogViewModel
    {
        private readonly IUserStore _userStore;
        private readonly IUserRepository _userRepo;

        public AccountModificationMenuViewModel(IUserStore userStore, IUserRepository userRepository)
        {
            _userStore = userStore;
            _userRepo = userRepository;

            ChangeUsernameCommand = new RelayCommand(o => ChangeUsername());
            ChangePasswordCommand = new RelayCommand(o => ChangePassword(o));
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
            }
        }

        private void RequestRole(object o)
        {
            throw new NotImplementedException();
        }

        private async void ChangeUsername()
        {
            if (_userStore.CurrentUser.Username == NewUsername || string.IsNullOrEmpty(NewUsername) || string.IsNullOrWhiteSpace(NewUsername) || NewUsername.Length < 5) return; // TODO: temp

            _userStore.Update(user => user.Username = NewUsername);
            await _userRepo.UpdateUserAsync(_userStore.CurrentUser);

            RequestClose?.Invoke();
        }

        private void ChangePassword(object o)
        {
            throw new NotImplementedException();
        }

        public string PopupMode => _userStore.AppendingPropertyChange;

        public ICommand ChangeUsernameCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand RequestRoleCommand { get; }
        public ICommand NavigateBackCommand { get; }
    }
}
