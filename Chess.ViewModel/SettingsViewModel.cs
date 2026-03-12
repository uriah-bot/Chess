using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
		private int _volume;
		public int Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				_volume = value;
				OnPropertyChanged(nameof(Volume));
			}
		}

		private bool _muteRadioDuringGame;
		public bool MuteRadioDuringGame
		{
			get
			{
				return _muteRadioDuringGame;
			}
			set
			{
				_muteRadioDuringGame = value;
				OnPropertyChanged(nameof(MuteRadioDuringGame));
			}
		}

		private bool _playSoundOnMove;
		public bool PLaySoundOnMove
		{
			get
			{
				return _playSoundOnMove;
			}
			set
			{
				_playSoundOnMove = value;
				OnPropertyChanged(nameof(PLaySoundOnMove));
			}
		}

		private bool _showHighlights;
		public bool ShowHighlights
		{
			get
			{
				return _showHighlights;
			}
			set
			{
				_showHighlights = value;
				OnPropertyChanged(nameof(ShowHighlights));
			}
		}

		private bool _displayCoordinates;
		public bool DisplayCoordinates
		{
			get
			{
				return _displayCoordinates;
			}
			set
			{
				_displayCoordinates = value;
				OnPropertyChanged(nameof(DisplayCoordinates));
			}
		}
	}
}
