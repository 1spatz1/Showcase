using ErrorOr;

namespace Showcase.Domain.Common.Errors;

public static partial class Errors
{
    public static class Admin
    {
        public static Error InvalidAction = Error.Unauthorized(
            $"{nameof(Authentication)}.{nameof(InvalidAction)}",
            "Invalid action");
        
    }
}