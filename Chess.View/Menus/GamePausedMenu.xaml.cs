using System.Windows;
using System.Windows.Controls;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for GamePausedMenu.xaml
    /// </summary>
    public partial class GamePausedMenu : UserControl
    {
        public event Action<MenuOption> OptionSelected;
        public GamePausedMenu()
        {
            InitializeComponent();
        }

        private void Resign_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Resign);
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Continue);
        }
    }
}
