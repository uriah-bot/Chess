using Chess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ViewModel.Stores
{
    public interface IGameConfigurationStore
    {
        GameMode Mode { get; set; }
        PlayerColor UserColor { get; set; }
        Game ConfigurateGame();
    }

    public class GameConfigurationStore : IGameConfigurationStore
    {
        public GameMode Mode { get; set; } = GameMode.Classical;
        public PlayerColor UserColor { get; set; } = PlayerColor.White;

        public Game ConfigurateGame()
        {
            Game game = new Game(PlayerColor.White, UserColor == PlayerColor.White ? Board.Initial() : Board.InitialInverse());
            game.Mode = Mode;

            return game;
        }
    }
}
