using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BCSH2_SEM.ViewModel.Converters
{
    public class InverseBoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = !(value is bool && (bool)value);
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((value is Visibility) && ((Visibility)value == Visibility.Visible));
        }
    }
}