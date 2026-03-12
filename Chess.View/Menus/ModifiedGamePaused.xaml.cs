using System.Windows;
using System.Windows.Controls;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for ModifiedGamePaused.xaml
    /// </summary>
    public partial class ModifiedGamePaused : UserControl
    {
        public event Action<MenuOption> OptionSelected; 
        public ModifiedGamePaused()
        {
            InitializeComponent();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Restart);
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Continue);
        }
        // TODO: implement in main window (after knowing if pvp or pve sent it)
    }
}
