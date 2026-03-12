using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class RegisterViewModel : ViewModelBase
    {
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

		private string _email;
		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				_email = value;
				OnPropertyChanged(nameof(Email));
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
        // for preventing double clicks when requesting registration

        private ICommand RegisterCommand { get; }

        //public RegisterViewModel()
        //{
        //    RegisterCommand = new RelayCommand(RequestRegistrationAsync);
        //}
    }
}
