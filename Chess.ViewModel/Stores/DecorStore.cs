using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ViewModel.Stores
{
    public interface IDecorStore
    {
        Uri CurrentSong { get; set; }
        double CurrentVolume { get; set; }
        event Action CurrentSongChanged;
        event Action VolumeChanged;

        List<Uri> CurrentPieces { get; set; }
        event Action CurrentPiecesChanged;

        Uri CurrentBoard { get; set; }
        event Action CurrentBoardChanged;
    }

    public class DecorStore : IDecorStore
    {
        private readonly IUserStore _userStore;

        public DecorStore(IUserStore userStore)
        {
            _userStore = userStore;

            //_currentVolume = _userStore.CurrentUser.Settings.Volume;
        }

        private Uri _currentSong;
        public Uri CurrentSong
        {
            get => _currentSong;
            set
            {
                _currentSong = value;
                CurrentSongChanged?.Invoke();
            }
        }
        public event Action CurrentSongChanged;

        private List<Uri> _currentPieces;
        public List<Uri> CurrentPieces
        {
            get => _currentPieces;
            set
            {
                _currentPieces = value;
                CurrentPiecesChanged?.Invoke();
            }
        }
        public event Action CurrentPiecesChanged;

        private Uri _currentBoard;
        public Uri CurrentBoard
        {
            get => _currentBoard;
            set
            {
                _currentBoard = value;
                CurrentBoardChanged?.Invoke();
            }
        }

        private double _currentVolume;
        public double CurrentVolume
        {
            get => _currentVolume;
            set
            {
                _currentVolume = value;
                VolumeChanged?.Invoke();
            }
        }

        public event Action CurrentBoardChanged;
        public event Action VolumeChanged;
    }
}
