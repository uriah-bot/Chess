using Chess.Model;
using Chess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for ChessBoard.xaml
    /// </summary>
    public partial class ChessBoard : UserControl
    {
        private readonly Image[,] PieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

        private Game Game;
        private Position selectedPosition = null;
        List<ModifierType> chosenRules = new List<ModifierType>()
        {
            ModifierType.KingPromotion,
            ModifierType.Poof
        };

        private Move lastMove = null; // only for UI
        private GameViewModel GameViewModel = new GameViewModel();

        public ChessBoard()
        {
            InitializeComponent();
            DataContext = GameViewModel;
            InitializeBoard();

            Game = new Game(PlayerColor.White, Board.Initial());
            Game.StartMatch(chosenRules);

            DrawBoard(Game.Board);
            SetCursor(Game.CurrentPlayer);
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var image = new Image();
                    PieceImages[row, col] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle highlight = new Rectangle();
                    highlights[row, col] = highlight;
                    HighlightGrid.Children.Add(highlight);
                }
            }
        }

        private void DrawBoard(Board board)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var piece = board[row, col];
                    PieceImages[row, col].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuOpen())
            {
                return; // Ignore chess board clicks when a menu is open
            }

            Point point = e.GetPosition(BoardGrid);
            Position pos = PointToCoordinates(point);

            if (selectedPosition == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }

        private void OnFromPositionSelected(Position position) // helper
        {
            HideLastMoveHighlight();
            IEnumerable<Move> moves = Game.LegalMovesForPiece(position);
            if (moves.Any())
            {
                selectedPosition = position;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        private void OnToPositionSelected(Position toPosition) // helper
        {
            selectedPosition = null;
            HideHighlights();

            if (moveCache.TryGetValue(toPosition, out Move move))
            {
                if (move.Type == MoveType.Promotion)
                {
                    HandlePromotion(move.FromPosition, move.ToPosition);
                }
                else
                {
                    HandleMove(move);
                }
            }
            ShowLastMoveHighlight(move);
        }

        private void HandlePromotion(Position fromPosition, Position toPosition) // helper
        {
            PieceImages[toPosition.Row, toPosition.Column].Source = Images.GetImage(Game.CurrentPlayer, PieceType.Pawn);
            PieceImages[fromPosition.Row, fromPosition.Column].Source = null;

            PromotionMenu promotionMenu = new PromotionMenu(Game.CurrentPlayer);
            MenuContainer.Content = promotionMenu;

            promotionMenu.PieceSelected += pieceType =>
            {
                MenuContainer.Content = null;
                Move promotionMove = new PawnPromotion(fromPosition, toPosition, pieceType);
                HandleMove(promotionMove);
            };
        }

        private void HandleMove(Move move) // helper
        {
            Game.MakeMove(move);
            DrawBoard(Game.Board);
            SetCursor(Game.CurrentPlayer);

            if (Game.IsGameOver())
            {
                ShowGameOver();
            }
        }

        private Position PointToCoordinates(Point point) // helper
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int column = (int)(point.X / squareSize);
            return new Position(row, column);
        }

        private void CacheMoves(IEnumerable<Move> moves) // helper
        {
            moveCache.Clear();
            foreach (Move move in moves)
            {
                moveCache[move.ToPosition] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position toPosition in moveCache.Keys)
            {
                highlights[toPosition.Row, toPosition.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position toPosition in moveCache.Keys)
            {
                highlights[toPosition.Row, toPosition.Column].Fill = Brushes.Transparent;
            }
        }

        private void ShowLastMoveHighlight(Move move)
        {
            if (move == null)
            {
                return;
            }

            Color color = Color.FromArgb(50, 91, 59, 252);

            highlights[move.FromPosition.Row, move.FromPosition.Column].Fill = new SolidColorBrush(color);
            highlights[move.ToPosition.Row, move.ToPosition.Column].Fill = new SolidColorBrush(color);

            lastMove = move; // bad practice to set it here but it's only for UI handling ig
        }

        private void HideLastMoveHighlight()
        {
            if (lastMove == null)
            {
                return;
            }

            highlights[lastMove.FromPosition.Row, lastMove.FromPosition.Column].Fill = Brushes.Transparent;
            highlights[lastMove.ToPosition.Row, lastMove.ToPosition.Column].Fill = Brushes.Transparent;
        }

        private void SetCursor(PlayerColor player)
        {
            if (player == PlayerColor.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }

        private bool IsMenuOpen()
        {
            return MenuContainer.Content != null;
        }

        private void ShowGameOver()
        {
            GameOverMenu gameOverMenu = new GameOverMenu(Game);
            MenuContainer.Content = gameOverMenu;

            gameOverMenu.OptionSelected += option =>
            {
                if (option == MenuOption.Exit)
                {
                    var home = new AppBase();
                    home.Show();
                    Window.GetWindow(this).Close();
                }
                else if (option == MenuOption.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
            };
        }

        private void RestartGame()
        {
            selectedPosition = null;
            HideHighlights();
            HideLastMoveHighlight();
            moveCache.Clear();
            // needed to happen before new game is made (for updated highlights & logic)

            Game = new Game(PlayerColor.White, Board.Initial());
            Game.StartMatch(chosenRules);
            DrawBoard(Game.Board);
            SetCursor(Game.CurrentPlayer);
        }

        private void PauseMenu_Click(object sender, RoutedEventArgs e)
        {
            if (!IsMenuOpen())
            {
                ShowPauseMenu();
            }
        }

        private void ShowPauseMenu()
        {
            GamePausedMenu PauseMenu = new GamePausedMenu();
            MenuContainer.Content = PauseMenu;

            PauseMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;

                if (option == MenuOption.Resign)
                {
                    Game.HasResigned();
                    ShowGameOver();
                }
            };
        }
    }
}
