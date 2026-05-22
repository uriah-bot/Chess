using Chess.Model;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace Chess.View.Converters
{
    public class PlayerColorToCursorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlayerColor playerColor)
            {
                return playerColor == PlayerColor.White && playerColor != PlayerColor.None ? ChessCursors.WhiteCursor : ChessCursors.BlackCursor;
            }

            return Cursors.Arrow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
