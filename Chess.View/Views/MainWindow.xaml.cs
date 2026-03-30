using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Chess.Model;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private ChessBoard _ChessBoard;
        public MainWindow()
        {
            InitializeComponent();
            //_ChessBoard = new ChessBoard();
            //MainContentArea.Content = _ChessBoard;
        }
    }
}