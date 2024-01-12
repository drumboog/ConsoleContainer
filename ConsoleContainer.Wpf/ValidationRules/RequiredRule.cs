using System.Globalization;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.ValidationRules
{
    public class RequiredRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(false, "Value must be provided");
            }
            return ValidationResult.ValidResult;
        }
    }
}
