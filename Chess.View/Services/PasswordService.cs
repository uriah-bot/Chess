using System.Windows;
using System.Windows.Controls;

namespace Chess.View
{
    public class PasswordService
    {
        public static readonly DependencyProperty BoundPasswordProperty =
        DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordService),
            new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static void SetBoundPassword(DependencyObject dp, string value) => dp.SetValue(BoundPasswordProperty, value);
        public static string GetBoundPassword(DependencyObject dp) => (string)dp.GetValue(BoundPasswordProperty);

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

                // specifically for when shown password is updated. bad practice tho
                string newPassword = (string)e.NewValue;
                if (passwordBox.Password != newPassword)
                {
                    passwordBox.Password = newPassword ?? string.Empty;
                }

                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetBoundPassword(passwordBox, passwordBox.Password);
        }
    }
}
