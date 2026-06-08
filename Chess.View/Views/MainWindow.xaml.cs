using Chess.ViewModel;
using System.Windows;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Radio_MediaEnded(object sender, RoutedEventArgs e)
        {
            Radio.Position = TimeSpan.Zero;
        }

        private void Radio_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.CurrentViewModel is GameViewModel gvm && gvm.ShouldActivateRadio)
            {
                Radio.Play();
            }
        }
    }
}