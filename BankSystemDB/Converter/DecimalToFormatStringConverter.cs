using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Homework_18.Converter
{
    class DecimalToFormatStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string specifier = "C";
            culture = CultureInfo.CurrentCulture;
            return ((decimal)value).ToString(specifier, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
