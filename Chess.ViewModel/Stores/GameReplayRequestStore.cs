using Chess.Model;

namespace Chess.ViewModel.Stores
{
    public interface IGameReplayRequestStore
    {
        public bool IsReplayRequested
        {
            get => RequestedGame != null;
            set
            {
                RequestedGame = null;
            }
        }
        GameEntity RequestedGame { get; set; }
    }

    public class GameReplayRequestStore : IGameReplayRequestStore
    {
        public bool IsReplayRequested
        {
            get => RequestedGame != null;
            set
            {
                RequestedGame = null;
            }
        }
        public GameEntity RequestedGame { get; set; }
    }
}
