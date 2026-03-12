using System.Windows.Controls;
using Chess.ViewModel;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
}
