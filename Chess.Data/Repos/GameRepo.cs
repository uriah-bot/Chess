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
                    string rawFENs = game["GameFENs"].ToString();
                    // removes empty strings returned   e.g ",," with ',' Split
                    List<string> fens = rawFENs.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();

                    GameEntity entity = new GameEntity
                    {
                        Id = (int)game["ID"],
                        GameFENs = fens,
                        Username = game["Username"].ToString(),
                        UserPlayedAs = Enum.TryParse<PlayerColor>(game["UserPlayedAs"]?.ToString(), out var color) ? color : null,
                        Result = game["Result"].ToString(),
                        DatePlayed = new DateTime((long)game["Date"]),
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
            string sql = "INSERT INTO Games (Username, GameFENs, GameMode, UserPlayedAs, BotRating, Result, DatePlayed) VALUES (?, ?, ?, ?, ?, ?, ?)";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@username", newGame.Username),
                new OleDbParameter("@gameFENs", newGame.GameFENs),
                new OleDbParameter("@gameMode", newGame.GameMode),
                new OleDbParameter("@userPlayedAs", newGame.UserPlayedAs),
                new OleDbParameter("@botRating", newGame.BotRating),
                new OleDbParameter("@result", newGame.Result.ToString()),
                new OleDbParameter("@date", newGame.DatePlayed)
            );
        }

        public async Task DeleteAllUserGamesAsync(UserEntity user)
        {
            string sql = "DELETE FROM Games WHERE UserID =?";

            await DbConnectionProvider.ExecuteQueryAsync(sql, new OleDbParameter("@userId", user.Id));
        }
    }
}
