using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConsoleContainer.Wpf.ValueConverters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        public Visibility NullValue { get; set; }
        public Visibility NonNullValue { get; set; }

        public NullToVisibilityConverter()
        {
            // set defaults
            NonNullValue = Visibility.Visible;
            NullValue = Visibility.Collapsed;
        }

        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null ? NullValue : NonNullValue;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
