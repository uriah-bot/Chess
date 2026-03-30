using Chess.Model;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Chess.View.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not UserRole role || parameter is null)
                return Visibility.Collapsed;

            var allowedRoles = parameter.ToString()
                                    .Split('|')
                                    .Select(r => Enum.Parse<UserRole>(r));

            return allowedRoles.Contains(role) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
