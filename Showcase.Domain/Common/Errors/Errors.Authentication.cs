using ErrorOr;

namespace Showcase.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error UsernameTaken = Error.Conflict($"{nameof(Authentication)}.{nameof(UsernameTaken)}",
            "A user with this username already exists");

        public static Error EmailTaken = Error.Conflict($"{nameof(Authentication)}.{nameof(EmailTaken)}",
            "A user with this email already exists");

        public static Error InvalidCredentials = Error.Unauthorized(
            $"{nameof(Authentication)}.{nameof(InvalidCredentials)}",
            "Invalid username or password");
        
        public static Error PasswordsDoNotMatch = Error.Conflict(
            $"{nameof(Authentication)}.{nameof(PasswordsDoNotMatch)}",
            "Passwords do not match");
        
        public static Error UserLockedOut = Error.Unauthorized(
            $"{nameof(Authentication)}.{nameof(UserLockedOut)}",
            "User is currently locked out");

        public static Error InvalidToken =
            Error.Unauthorized($"{nameof(Authentication)}.{nameof(InvalidToken)}", "Invalid token");
    }
}