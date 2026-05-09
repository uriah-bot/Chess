using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chess.Model;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for Adventure.xaml
    /// </summary>
    public partial class Adventure : UserControl
    {
        public Adventure()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollerV)
            {
                // Redirect vertical wheel to horizontal scrolling
                scrollerV.ScrollToHorizontalOffset(scrollerV.HorizontalOffset - e.Delta * 0.3);
                e.Handled = true;
            }
        }
    }
}
