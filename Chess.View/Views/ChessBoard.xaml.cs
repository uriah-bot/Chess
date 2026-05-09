using Chess.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        private Position rightClickStart = null;
        private List<Position> markedSquares = new List<Position>();
        private List<(Position From, Position To)> plannedArrows = new List<(Position, Position)>();

        private Game Game;
        private Position selectedPosition = null;
        List<ActiveModifier> chosenRules = new List<ActiveModifier>()
        {
            new ActiveModifier{Modifier = ModifierType.TimeLimit, SelectedParameter="1"},
            new ActiveModifier{Modifier = ModifierType.Poof, SelectedParameter="1"}
        };

        private Move lastMove = null; // only for UI

        public ChessBoard()
        {
            InitializeComponent();
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

            if (markedSquares.Any() || plannedArrows.Any())
            {
                markedSquares.Clear();
                plannedArrows.Clear();
                DrawingCanvas.Children.Clear();
            }

            Point point = e.GetPosition(BoardGrid);
            Position pos = PointToCoordinates(point);

            if (selectedPosition == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                HideHighlightSelectedPosition(selectedPosition);
                OnToPositionSelected(pos);
            }
        }

        private void OnFromPositionSelected(Position position) // helper
        {
            IEnumerable<Move> moves = Game.LegalMovesForPiece(position);
            if (moves.Any())
            {
                selectedPosition = position;
                CacheMoves(moves);
                ShowHighlights();
            }
            HighlightSelectedPosition(selectedPosition);
        }

        private void OnToPositionSelected(Position toPosition) // helper
        {
            selectedPosition = null;
            HideHighlights();

            if (!moveCache.TryGetValue(toPosition, out Move move))
            {
                OnFromPositionSelected(toPosition);
                return;
            }

            HideLastMoveHighlight();

            if (move.Type == MoveType.Promotion)
            {
                HandlePromotion(move.FromPosition, move.ToPosition);
            }
            else
            {
                HandleMove(move);
            }

            ShowLastMoveHighlight(move);
        }

        private void HandlePromotion(Position fromPosition, Position toPosition) // helper
        {
            PieceImages[toPosition.Row, toPosition.Column].Source = Images.GetImage(Game.CurrentPlayer, PieceType.Pawn);
            PieceImages[fromPosition.Row, fromPosition.Column].Source = null;

            //PromotionMenu promotionMenu = new PromotionMenu(Game.CurrentPlayer);
            //MenuContainer.Content = promotionMenu;

            //promotionMenu.PieceSelected += pieceType =>
            //{
            //    MenuContainer.Content = null;
            //    Move promotionMove = new PawnPromotion(fromPosition, toPosition, pieceType);
            //    HandleMove(promotionMove);
            //};
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

        private void HighlightSelectedPosition(Position pos)
        {
            if (pos == null)
            {
                return;
            }

            Color color = Color.FromArgb(150, 170, 94, 220);

            highlights[pos.Row, pos.Column].Fill = new SolidColorBrush(color);
        }

        private void HideHighlightSelectedPosition(Position pos)
        {
            if (pos == null)
            {
                return;
            }

            highlights[pos.Row, pos.Column].Fill = Brushes.Transparent;
        }

        private void ShowLastMoveHighlight(Move move)
        {
            if (move == null)
            {
                return;
            }

            Color color = Color.FromArgb(150, 84, 198, 247);

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
            //GameOverMenu gameOverMenu = new GameOverMenu(Game);
            //MenuContainer.Content = gameOverMenu;

            //gameOverMenu.OptionSelected += option =>
            //{
            //    if (option == MenuOption.Exit)
            //    {
            //        var home = new AppBase();
            //        home.Show();
            //        Window.GetWindow(this).Close();
            //    }
            //    else if (option == MenuOption.Restart)
            //    {
            //        MenuContainer.Content = null;
            //        RestartGame();
            //    }
        }

        private void RestartGame()
        {
            // simple highlights
            selectedPosition = null;
            HideHighlights();
            HideLastMoveHighlight();

            // planned highlights or arrows
            markedSquares.Clear();
            plannedArrows.Clear();
            DrawingCanvas.Children.Clear();

            moveCache.Clear();
            
            // selected position after restart
            foreach (var rec in highlights) rec.Fill = Brushes.Transparent;

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
            //GamePausedMenu PauseMenu = new GamePausedMenu();
            //MenuContainer.Content = PauseMenu;

            //PauseMenu.OptionSelected += option =>
            //{
            //    MenuContainer.Content = null;

            //    if (option == MenuOption.Resign)
            //    {
            //        Game.HasResigned();
            //        ShowGameOver();
            //    }
            //};
        }

        private void BoardGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuOpen() || rightClickStart == null) return;

            Point point = e.GetPosition(BoardGrid);
            Position rightClickEnd = PointToCoordinates(point);

            if (rightClickStart == rightClickEnd)
            {
                // CLICKED A SINGLE SQUARE now mark it
                bool removed = markedSquares.RemoveAll(p => p == rightClickEnd) > 0;
                if (!removed)
                {
                    markedSquares.Add(rightClickEnd);
                }
            }
            else
            {
                // DRAGGED BETWEEN SQUARES = arrow
                bool removed = plannedArrows.RemoveAll(a => a.From == rightClickStart && a.To == rightClickEnd) > 0;
                if (!removed)
                {
                    plannedArrows.Add((rightClickStart, rightClickEnd));
                }
            }

            rightClickStart = null;
            DrawRightClickDrawings();
        }

        private void DrawRightClickDrawings()
        {
            // Wipe the canvas clean before redrawing
            DrawingCanvas.Children.Clear();
            double squareSize = BoardGrid.ActualWidth / 8;

            // DRAW RED SQUARES
            SolidColorBrush markedBrush = new SolidColorBrush(Color.FromArgb(130, 246, 31, 31));
            foreach (Position pos in markedSquares)
            {
                Rectangle rect = new Rectangle
                {
                    Width = squareSize,
                    Height = squareSize,
                    Fill = markedBrush
                };
                // Position rectangle over the square
                Canvas.SetLeft(rect, pos.Column * squareSize);
                Canvas.SetTop(rect, pos.Row * squareSize);
                DrawingCanvas.Children.Add(rect);
            }

            // DRAW THE ARROWS
            SolidColorBrush arrowBrush = new SolidColorBrush(Color.FromArgb(180, 255, 170, 0));
            foreach (var arrow in plannedArrows)
            {
                double startX = (arrow.From.Column * squareSize) + (squareSize / 2);
                double startY = (arrow.From.Row * squareSize) + (squareSize / 2);
                double endX = (arrow.To.Column * squareSize) + (squareSize / 2);
                double endY = (arrow.To.Row * squareSize) + (squareSize / 2);

                // Draw the Triangle Arrowhead
                double angle = Math.Atan2(endY - startY, endX - startX);
                double headLength = 25;
                double headAngle = Math.PI / 6;

                // Draw the main line
                Line line = new Line
                {
                    X1 = startX,
                    Y1 = startY,
                    X2 = endX - 0.8 * headLength * Math.Cos(angle),
                    Y2 = endY - 0.8 * headLength * Math.Sin(angle),
                    Stroke = arrowBrush,
                    StrokeThickness = 12,
                    StrokeEndLineCap = PenLineCap.Flat,
                    StrokeStartLineCap = PenLineCap.Round
                };
                DrawingCanvas.Children.Add(line);

                Point p1 = new Point(endX, endY);
                Point p2 = new Point(endX - headLength * Math.Cos(angle - headAngle), endY - headLength * Math.Sin(angle - headAngle));
                Point p3 = new Point(endX - headLength * Math.Cos(angle + headAngle), endY - headLength * Math.Sin(angle + headAngle));

                Polygon arrowhead = new Polygon
                {
                    Points = new PointCollection { p1, p2, p3 },
                    Fill = arrowBrush
                };
                DrawingCanvas.Children.Add(arrowhead);
            }
        }

        private void BoardGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuOpen()) return;

            Point point = e.GetPosition(BoardGrid);
            rightClickStart = PointToCoordinates(point);
        }
    }
}
