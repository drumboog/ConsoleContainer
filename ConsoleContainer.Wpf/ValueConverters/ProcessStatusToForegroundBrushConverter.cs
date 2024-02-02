using ConsoleContainer.ProcessManagement;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ConsoleContainer.Wpf.ValueConverters
{
    [ValueConversion(typeof(ProcessState), typeof(Brush))]
    public sealed class ProcessStatusToForegroundBrushConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ProcessState state))
            {
                return value;
            }

            switch (state)
            {
                case ProcessState.Idle:
                    return new SolidColorBrush(Colors.Red);
                case ProcessState.Starting:
                    return new SolidColorBrush(Colors.Green) { Opacity = 0.6 };
                case ProcessState.Running:
                    return new SolidColorBrush(Colors.Green);
                case ProcessState.Stopping:
                    return new SolidColorBrush(Colors.IndianRed);
                default:
                    throw new Exception($"Unknown ProcessState: {state}");
            }
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
