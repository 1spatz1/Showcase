using ErrorOr;

namespace Showcase.Domain.Common.Errors;

public static partial class Errors
{
    public static class Admin
    {
        public static Error InvalidAction = Error.Unauthorized(
            $"{nameof(Authentication)}.{nameof(InvalidAction)}",
            "Invalid action");
        
        public static class GetAllusers
        {
            public static Error Error = Error.Unexpected($"{nameof(Admin)}.{nameof(GetAllusers)}.{nameof(Error)}", "An error occurred while processing your request");
        }
    }
}