using Chess.Model;
using Chess.ViewModel.ViewModelHelper;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class GameOverMenuViewModel : ViewModelBase, IDialogViewModel
    {
        public Action RequestClose { get; set; }

        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;
        private readonly IGameManagerService _gameManagerService;

        public string Winner { get; }
        public string WinReason { get; }

        public ICommand ExitCommand { get; }
        public ICommand PlayAgainCommand { get; }

        public GameOverMenuViewModel(INavigationService navigationService, IWindowService windowService, IGameManagerService gameManagerService)
        {
            _navigationService = navigationService;
            _windowService = windowService;
            _gameManagerService = gameManagerService;

            Winner = GetWinnerText(_gameManagerService.Game.Result.winner);
            WinReason = GetReason(_gameManagerService.Game.Result.reason, _gameManagerService.Game.CurrentPlayer);

            ExitCommand = new RelayCommand(o => ExitToApp());
            PlayAgainCommand = new RelayCommand(o => PlayAgain());
        }

        private void PlayAgain()
        {
            RequestClose?.Invoke();

            _navigationService.NavigateTo<GameViewModel>();
        }

        private void ExitToApp()
        {
            RequestClose?.Invoke();
            
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _windowService.SwitchWindow<AppBaseViewModel>();
            }), System.Windows.Threading.DispatcherPriority.Background); // <-- THIS IS THE MAGIC
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
                EndReason.TimeRanOut => $"{PlayerString(currentPlayer).ToUpper()} RAN OUT OF TIME",
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
    }
}
