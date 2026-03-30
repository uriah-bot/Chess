using System.Globalization;
using System.Windows.Data;

namespace Chess.View.Converters
{
    public class ViewModelToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Does current VM match type passed in parameter?
            return value?.GetType() == (Type)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
