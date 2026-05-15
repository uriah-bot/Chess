namespace Chess.ViewModel.Stores
{
    public interface IDecorStore
    {
        Uri CurrentSong { get; set; }
        double CurrentVolume { get; set; }
        event Action CurrentSongChanged;
        event Action VolumeChanged;
    }

    public class DecorStore : IDecorStore
    {
        private readonly IUserStore _userStore;

        public DecorStore(IUserStore userStore)
        {
            _userStore = userStore;
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
        public event Action VolumeChanged;
    }
}
