using ConsoleContainer.Wpf.Exceptions;
using System.Runtime.CompilerServices;

namespace ConsoleContainer.Wpf.Extensions
{
    public static class ValidationExtensions
    {
        public static string Required(this string? val, [CallerMemberName] string? callerMemberName = null)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                throw new ValidationException(callerMemberName ?? "Unknown Property", "Value is required");
            }
            return val;
        }
    }
}
