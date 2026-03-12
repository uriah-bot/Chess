using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ViewModel
{
    public class AppBaseSidebarViewModel : ViewModelBase
    {
		private string _username;
		public string Username
		{
			get
			{
				return _username;
			}
			private set
			{
                _username = value;
                OnPropertyChanged(nameof(Username));
			}
		}
	}
}
