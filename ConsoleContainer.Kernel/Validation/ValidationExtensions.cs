using ConsoleContainer.Kernel.Exceptions;
using System.Runtime.CompilerServices;

namespace ConsoleContainer.Kernel.Validation
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

        public static T Required<T>(this T? val, [CallerMemberName] string? callerMemberName = null)
            where T : struct
        {
            if (val is null)
            {
                throw new ValidationException(callerMemberName ?? "Unknown Property", "Value is required");
            }
            return val.Value;
        }
    }
}
