using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ViewModel
{
    public class StatsViewModel : ViewModelBase
    {
		private int _wins;
		public int Wins
		{
			get
			{
				return _wins;
			}
			set
			{
				_wins = value;
				OnPropertyChanged(nameof(Wins));
			}
		}

		private int _draws;
		public int Draws
		{
			get
			{
				return _draws;
			}
			set
			{
				_draws = value;
				OnPropertyChanged(nameof(Draws));
			}
		}

		private int _losses;
		public int Losses
		{
			get
			{
				return _losses;
			}
			set
			{
				_losses = value;
				OnPropertyChanged(nameof(Losses));
			}
		}

		private int _totalMatches;
		public int TotalMatches
		{
			get
			{
				return _totalMatches;
			}
			set
			{
				_totalMatches = value;
				OnPropertyChanged(nameof(TotalMatches));
			}
		}
	}
}
