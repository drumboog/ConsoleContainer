using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ConsoleContainer.Wpf
{
    internal class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private Dictionary<string, object?> propertyValues = new Dictionary<string, object?>();

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();

                var pi = GetType().GetProperty(columnName);
                if (pi is null)
                {
                    return string.Empty;
                }
                if (Validator.TryValidateProperty(pi.GetValue(this), new ValidationContext(this) { MemberName = columnName }, validationResults))
                {
                    return string.Empty;
                }

                return validationResults.FirstOrDefault()?.ErrorMessage ?? string.Empty;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public T? GetProperty<T>([CallerMemberName] string propertyName = "")
        {
            return GetProperty<T>(default(T), propertyName);
        }

        public T? GetProperty<T>(T? defaultValue, [CallerMemberName] string propertyName = "")
        {
            return GetProperty(() => defaultValue, propertyName);
        }

        public T GetProperty<T>(Func<T> defaultValueFactory, [CallerMemberName] string propertyName = "")
        {
            if (!TryGetProperty<T>(propertyName, out var value))
            {
                return defaultValueFactory();
            }
            return value is T ? value : defaultValueFactory();
        }

        protected bool SetProperty<T>(T value, IEnumerable<string>? dependentPropertyNames = null, [CallerMemberName] string propertyName = "")
        {
            if (TryGetProperty<T>(propertyName, out var currentValue) &&
                EqualityComparer<T>.Default.Equals(currentValue, value))
            {
                return false;
            }
            propertyValues[propertyName] = value;
            OnPropertyChanged(propertyName);
            if (dependentPropertyNames is not null)
            {
                foreach(var name in dependentPropertyNames)
                {
                    OnPropertyChanged(name);
                }
            }
            return true;
        }

        private bool TryGetProperty<T>(string propertyName, out T? value)
        {
            if (!propertyValues.TryGetValue(propertyName, out var objValue))
            {
                value = default;
                return false;
            }
            value = (T?)objValue;
            return true;
        }
    }
}
