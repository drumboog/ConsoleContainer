using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.ValidationRules
{
    public class ValidPathRule : ValidationRule
    {
        public bool AllowFile { get; set; } = true;
        public bool AllowDirectory { get; set; } = true;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is null)
            {
                return ValidationResult.ValidResult;
            }

            if (!PathExists(value as string))
            {
                return new ValidationResult(false, "Path does not exit");
            }
            return ValidationResult.ValidResult;
        }

        private bool PathExists(string? path)
        {
            if (AllowFile && File.Exists(path))
            {
                return true;
            }
            if (AllowDirectory && Directory.Exists(path))
            {
                return true;
            }
            return false;
        }
    }
}
