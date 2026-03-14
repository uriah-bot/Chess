using Chess.Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    public class GameRepo : IGameRepository
    {
        public Task<GameEntity> GetGameByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task AddGameAsync(GameEntity newGame)
        {
            string sql = "INSERT INTO Games (UserId, GameMode, UserPlayedAs, BotRating, Result, DatePlayed) VALUES (?, ?, ?, ?, ?)";

            await DbConnectionProvider.ExecuteCommandAsync(sql,
                new OleDbParameter("@userId", newGame.UserId),
                new OleDbParameter("@gameMode", newGame.GameMode),
                new OleDbParameter("@userPlayedAs", newGame.UserPlayedAs),
                new OleDbParameter("@botRating", newGame.BotRating),
                new OleDbParameter("@result", newGame.Result.ToString()),
                new OleDbParameter("@date", newGame.DatePlayed)
            );
        }
    }
}
