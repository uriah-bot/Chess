using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Chess.View.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string || value == null)
                return Brushes.Transparent;

            return (Brush)new BrushConverter().ConvertFromString(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
