using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Model;

namespace Chess.ViewModel
{
    public partial class GameViewModel : ViewModelBase
    {
		private string _username; // TODO: Remove (test)
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

		private bool _isClassicalGame = true;
		public bool IsClassicalGame
		{
			get
			{
				return _isClassicalGame;
			}
			set
			{
				_isClassicalGame = value;
				OnPropertyChanged(nameof(IsClassicalGame));
			}
		}
	}
}
