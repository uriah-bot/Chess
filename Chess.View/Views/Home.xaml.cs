using System.Windows.Controls;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public event EventHandler AdventureRequested;
        public event EventHandler ClassicalRequested;

        public Home()
        {
            InitializeComponent();
        }

        private void Temp(object sender, System.Windows.RoutedEventArgs e)
        {
            AdventureRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ClassicalStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ClassicalRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
