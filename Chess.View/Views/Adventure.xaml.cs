using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chess.Model;
using static Chess.View.ModifierDetails;

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

        private void OpenInfo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string modifierName = btn.Tag.ToString();
                string safeEnumString = modifierName.Replace(" ", "");

                if (modifierName == "The King's Journey") // Edge case where name != enumName
                {
                    safeEnumString = "KingPromotion";
                }

                if (Enum.TryParse(safeEnumString, out ModifierType selectedModifier))
                {
                    PopupIcon.Text = GetIcon(selectedModifier);
                    PopupIcon.FontFamily = GetFontFamily(selectedModifier);
                    PopupIcon.Foreground = GetIconColor(selectedModifier);
                    PopupTitle.Text = modifierName;
                    PopupType.Text = GetModifierFamily(selectedModifier);
                    PopupDuration.Text = GetDuration(selectedModifier);
                    PopupCost.Text = "1 Point";
                    PopupDescription.Text = GetDescription(selectedModifier);
                }

                InfoOverlay.Visibility = Visibility.Visible;
            }
        }

        private void CloseOverlay_Click(object sender, RoutedEventArgs e)
        {
            InfoOverlay.Visibility = Visibility.Collapsed;
        }
    }
}
