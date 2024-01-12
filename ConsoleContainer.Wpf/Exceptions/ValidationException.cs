namespace ConsoleContainer.Wpf.Exceptions
{
    public class ValidationException : Exception
    {
        public string PropertyName { get; }
        public string ValidationError { get; }

        public ValidationException(string propertyName, string validationError)
            : base($"Validation failed on property {propertyName}: {validationError}")
        {
            PropertyName = propertyName;
            ValidationError = validationError;
        }
    }
}
