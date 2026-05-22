using Chess.Model;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class GameEntityViewModel : ViewModelBase
    {
        public ICommand ReplayCommand { get; }

        public List<Move> Moves { get; }
        public string GameMode { get; }
        public string AIName { get; }
        public string EloDelta { get; }
        public string Result { get; }
        public string Date { get; }
        public string UserColor { get; }
        public string ResultColor
        {
            get
            {
                return Result switch
                {
                    "Win" => "ForestGreen",
                    "Loss" => "MediumVioletRed",
                    "Draw" => "LightGray",
                    _ => "Gray"
                };
            }
        }

        public GameEntityViewModel(GameEntity game, Action replayAction)
        {
            GameMode = game.BotRating == null ? "Friendly Battle" : "Player vs AI";
            AIName = game.BotRating == null ? string.Join(", ", game.Modifiers.Select(m => m != ModifierType.Empty ? m.ToString() : string.Empty)) : "Stockfish (" + game.BotRating.ToString() + ")";
            UserColor = game.BotRating == null ? string.Empty : game.UserPlayedAs.ToString();
            EloDelta = game.EloDelta.HasValue && game.EloDelta >= 0 ? $"+{game.EloDelta}" : game.EloDelta.ToString();
            Result = game.BotRating == null ? "" : game.Result.ToString();
            Date = game.DatePlayed.ToString("yy-MM-dd--hh--mm");

            ReplayCommand = new RelayCommand(o => replayAction());
        }
    }
}
