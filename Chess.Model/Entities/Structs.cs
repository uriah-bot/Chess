using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public readonly record struct GameRecord
    {
        public GameRecord(GameEntity game)
        {
            GameMode = game.BotRating == null ? "Player vs Player" : "Player vs AI";
            AIName = game.BotRating == null ? string.Empty : "Stockfish (" + game.BotRating.ToString() + ")";
            UserColor = game.BotRating == null ? string.Empty : game.UserPlayedAs.ToString();
            Result = game.BotRating == null ? "(friendly game)" : game.Result.ToString();
            Date = game.DatePlayed.ToString("yy-MM-dd--hh--mm");
        }

        public string GameMode { get; }
        public string AIName { get; }
        public string Result { get; }
        public string Date { get; }
        public string UserColor { get; }
        public string ResultColor
        {
            get
            {
                return Result switch
                {
                    "Win" => "Green",
                    "Loss" => "Red",
                    "Draw" => "Yellow",
                    _ => "Black"
                };
            }
        }
    }

    public record struct LeaderboardEntry
    {
        public LeaderboardEntry(UserEntity user)
        {
            Username = user.Username;
            Elo = user.Elo;
            Wins = user.Wins;
        }

        public LeaderboardEntry(string Username, int Elo, int Wins, bool IsCurrentUser)
        {
            this.Username = Username;
            this.Elo = Elo;
            this.Wins = Wins;
            this.IsCurrentUser = IsCurrentUser;
        }

        public string Username { get; set; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public bool IsCurrentUser { get; set; }
    }

    public record ModifierData
    {
        public string Name { get; set; }
        public string IconName { get; set; }
        public string IconHexColor { get; set; }
        public string FontFamilyName { get; set; }
        public string Type { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public bool IsDynamic { get; set; }
        public List<string> DynamicItems { get; set; }
    }

    public record ActiveModifier
    {
        public ModifierType Modifier { get; set; }
        public string SelectedParameter { get; set; }
    }
}
