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

            if ((value as string).Contains(","))
            {
                // Expected format: "A,R,G,B" (e.g., "255,255,0,0" for opaque red) is byte and not standard int
                // less memory usage and faster parsing than ColorConverter for this specific format
                var colorParts = (value as string).Split(',', StringSplitOptions.TrimEntries);
                return (Brush)new SolidColorBrush(Color.FromArgb(
                    byte.Parse(colorParts[0]),
                    byte.Parse(colorParts[1]),
                    byte.Parse(colorParts[2]),
                    byte.Parse(colorParts[3])));
            }

            return (Brush)new BrushConverter().ConvertFromString(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
