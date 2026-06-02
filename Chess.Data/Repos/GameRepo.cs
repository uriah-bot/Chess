using Chess.Model;
using System.Data;
using System.Data.OleDb;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    public class GameRepo : IGameRepository
    {
        public async Task<List<GameEntity>> GetUserGamesAsync(UserEntity user)
        {
            string sql = "SELECT * FROM Games WHERE UserID =? ORDER BY ID DESC";

            DataTable dt = await DbConnectionProvider.ExecuteQueryAsync(sql, new OleDbParameter("@userId", user.Id));

            List<GameEntity> games = new List<GameEntity>();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow game in dt.Rows)
                {
                    string rawMoves = game["GameMoves"].ToString();
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
                        Result = game["Result"]?.ToString(),
                        DatePlayed = (DateTime)game["Date"],
                        BotRating = game["BotRating"] != DBNull.Value && game["BotRating"] != null ? (int)game["BotRating"] : null,
                    };

                    games.Add(entity);
                }

                return games;
            }

            return null;
        }

        public async Task AddUserGameAsync(GameEntity newGame)
        {
            string sql = "INSERT INTO Games (UserID, GameMoves, Modifiers, UserPlayedAs, BotRating, Result, [Date], EloDelta) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

            var gameMoves = string.Join("|", newGame.GameMoves);
            var modifiers = newGame.Modifiers != null ? string.Join("|", newGame.Modifiers.Select(m => m.ToString())) : string.Empty;

            // a lot of formatting because of the parsing, and having 2 different game-modes with requirements and null fields
            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@userId", newGame.UserId),
                new OleDbParameter("@gameMoves", gameMoves),
                new OleDbParameter("@modifiers", modifiers),
                new OleDbParameter("@userPlayedAs", newGame.UserPlayedAs.ToString()),
                new OleDbParameter("@botRating", (object)newGame.BotRating ?? DBNull.Value),
                new OleDbParameter("@result", (object)newGame.Result?.ToString() ?? DBNull.Value),
                new OleDbParameter("@date", newGame.DatePlayed.Date),
                new OleDbParameter("@eloDelta", (object)newGame.EloDelta ?? DBNull.Value)
            );
        }
    }
}
