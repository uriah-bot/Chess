using Chess.Model;
using System.Data;
using System.Data.OleDb;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    public class GameRepo : IGameRepository
    {
        public async Task<List<GameEntity>> GetGamesByUserAsync(UserEntity user)
        {
            string sql = "SELECT * FROM Games WHERE UserID =?";

            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql, new OleDbParameter("@userId", user.Id));

            List<GameEntity> games = new List<GameEntity>();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow game in dt.Rows)
                {
                    string rawMoves = game["GameFENs"].ToString();
                    // removes empty strings returned   e.g ",," with ',' Split
                    List<string> moves = rawMoves.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();

                    string rawMods = game["Modifiers"].ToString();
                    // removes empty strings returned   e.g ",," with ',' Split
                    List<ModifierType> mods = rawMods.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
                        .Select(m => Enum.TryParse<ModifierType>(m, out var mod) ? mod : ModifierType.Empty)
                        .ToList();

                    GameEntity entity = new GameEntity
                    {
                        Id = (int)game["ID"],
                        GameMoves = moves,
                        Modifiers = mods, 
                        EloDelta = game["EloDelta"] != DBNull.Value && game["EloDelta"] != null ? (int)game["EloDelta"] : null,
                        UserId = user.Id,
                        UserPlayedAs = Enum.TryParse<PlayerColor>(game["UserPlayedAs"]?.ToString(), out var color) ? color : null,
                        Result = game["Result"].ToString(),
                        //DatePlayed = new DateTime((long)game["Date"]),
                        BotRating = game["BotRating"] != DBNull.Value && game["BotRating"] != null ? (int)game["BotRating"] : null,
                    };

                    games.Add(entity);
                }

                return games;
            }

            return null;
        }

        public async Task AddGameAsync(GameEntity newGame)
        {
            string sql = "INSERT INTO Games (UserID, GameFENs, UserPlayedAs, BotRating, Result, DatePlayed, EloDelta) VALUES (?, ?, ?, ?, ?, ?, ?)";

            var gameMoves = string.Join("|", newGame.GameMoves);

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@userId", newGame.UserId),
                new OleDbParameter("@gameFENs", gameMoves),
                new OleDbParameter("@userPlayedAs", newGame.UserPlayedAs),
                new OleDbParameter("@botRating", newGame.BotRating),
                new OleDbParameter("@result", newGame.Result.ToString()),
                new OleDbParameter("@date", newGame.DatePlayed),
                new OleDbParameter("@eloDelta", newGame.EloDelta)
            );
        }
    }
}
