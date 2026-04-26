using System.Windows;
using System.Windows.Controls;
using Chess.Model;

namespace Chess.View.Menus
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<MenuOption> OptionSelected;
        // TODO: Differenciate between color and user on display
        /* TODO: Differenciate between pvp and pve so that pvp can't resign (only pause/restart)*/
        public GameOverMenu()
        {
            InitializeComponent();
            //Result result = game.Result;
            //WinnerTextBlock.Text = GetWinnerText(result.winner);
            //ReasonTextBlock.Text = GetReason(result.reason, game.CurrentPlayer);
        }

        private static string GetWinnerText(PlayerColor winner)
        {
            return winner switch
            {
                PlayerColor.White => "WHITE WINS!",
                PlayerColor.Black => "BLACK WINS!",
                _ => "IT'S A DRAW!"
            };
        }

        private static string GetReason(EndReason reason, PlayerColor currentPlayer)
        {
            return reason switch
            {
                EndReason.Stalemate => $"STALEMATE - {PlayerString(currentPlayer).ToUpper()} CAN'T MOVE",
                EndReason.Checkmate => $"{PlayerString(currentPlayer.Opponent()).ToUpper()} HAS CHECKMATED",
                EndReason.InsufficientMaterial => "DRAW BY INSUFFICIENT MATERIAL",
                EndReason.ThreefoldRepetition => "DRAW BY THREEFOLD REPETITION",
                EndReason.FiftyMoveRule => "DRAW BY FIFTY-MOVE RULE",
                EndReason.Resignation => "YOU LOST BY RESIGNATION",
                EndReason.KingPromotion => $"{PlayerString(currentPlayer.Opponent()).ToUpper()} PROMOTED THE KING",
                EndReason.NotEnoughPoofPieces => $"{PlayerString(currentPlayer).ToUpper()} RAN OUT OF POOF-ABLES",
                _ => ""
            };
        }

        private static string PlayerString(PlayerColor player)
        {
            return player switch
            {
                PlayerColor.White => "White",
                PlayerColor.Black => "Black",
                _ => ""
            };
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Exit);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Restart);
        }
    }
}
