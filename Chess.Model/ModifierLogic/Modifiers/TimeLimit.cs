using System.Timers;
using Timer = System.Timers.Timer;

namespace Chess.Model
{
    public class TimeLimit : IModifier
    {
        private Game _game;
        private int Time;
        private TimeSpan WhiteTime;
        private TimeSpan BlackTime;
        public Dictionary<PlayerColor, Timer> Timers { get; private set; } = new Dictionary<PlayerColor, Timer>();

        public List<ModifierType> Conflicts => null;

        public TimeLimit(int? param)
        {
            if (param != null)
            {
                Time = param.Value;
            }
            else
            {
                Time = AppConstants.TIME_LIMIT_DEFAULT_TIME;
            }

            WhiteTime = TimeSpan.FromMinutes(Time);
            BlackTime = TimeSpan.FromMinutes(Time);
        }

        public void Apply(Game game)
        {
            _game = game;

            _game.OnPieceMoved += ChangeActiveTimerTo;
            _game.OnBoardSetup += StartTimers;


            Timers.Add(PlayerColor.White, new Timer(1000));
            Timers.Add(PlayerColor.Black, new Timer(1000));

            Timers[PlayerColor.White].Elapsed += OnElapsed;
            Timers[PlayerColor.Black].Elapsed += OnElapsed;

            Timers[PlayerColor.White].AutoReset = true;
            Timers[PlayerColor.Black].AutoReset = true;
        }

        public void Remove(Game game)
        {
            _game.OnPieceMoved -= ChangeActiveTimerTo;
            _game.OnBoardSetup -= StartTimers;

            Timers[PlayerColor.White].Elapsed -= OnElapsed;
            Timers[PlayerColor.Black].Elapsed -= OnElapsed;

            Timers[PlayerColor.White].Stop();
            Timers[PlayerColor.Black].Stop();

            Timers[PlayerColor.White].Dispose();
            Timers[PlayerColor.Black].Dispose();

            Timers.Clear();
        }

        public void StartTimers(Board board)
        {
            Timers[_game.CurrentPlayer].Start();
        }

        public void ChangeActiveTimerTo(Move move)
        {
            if (_game.Result != null) return;

            Timers[_game.CurrentPlayer].Stop();
            Timers[_game.CurrentPlayer.Opponent()].Start();
        }

        public void OnElapsed(Object source, ElapsedEventArgs e)
        {
            if (_game.Result != null)
            {
                Timers[PlayerColor.White].Stop();
                Timers[PlayerColor.Black].Stop();
                return;
            }

            switch (_game.CurrentPlayer)
            {
                case PlayerColor.White:
                    WhiteTime = WhiteTime.Subtract(TimeSpan.FromSeconds(1));
                    _game.BroadcastModifierData("WhiteTime", WhiteTime.ToString(@"mm\:ss"));
                    if (WhiteTime <= TimeSpan.Zero)
                    {
                        _game.Result = Result.ModifiedWin(_game.CurrentPlayer.Opponent(), EndReason.TimeRanOut);
                        return;
                    }
                    break;
                case PlayerColor.Black:
                    BlackTime = BlackTime.Subtract(TimeSpan.FromSeconds(1));
                    _game.BroadcastModifierData("BlackTime", BlackTime.ToString(@"mm\:ss"));
                    if (BlackTime <= TimeSpan.Zero)
                    {
                        _game.Result = Result.ModifiedWin(_game.CurrentPlayer.Opponent(), EndReason.TimeRanOut);
                        return;
                    }
                    break;
            }
        }
    }
}
