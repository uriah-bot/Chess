using Chess.Model;
using System.Globalization;
using System.Windows.Data;

namespace Chess.View.Converters
{
    public class PieceToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Piece piece)
            {
                return Images.GetImage(piece); ;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => throw new NotImplementedException();
    }
}
