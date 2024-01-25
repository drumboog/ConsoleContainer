using System.Globalization;
using System.Windows.Data;

namespace ConsoleContainer.Wpf.ValueConverters
{
    [ValueConversion(typeof(double), typeof(double))]
    public sealed class AddValueConverter : IValueConverter
    {
        public double ValueToAdd { get; set; }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double val))
                return value;
            return val + ValueToAdd;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double val))
                return value;
            return val - ValueToAdd;
        }
    }
}
