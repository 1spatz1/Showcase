using ErrorOr;

namespace Showcase.Domain.Common.Errors;

public static partial class Errors
{
    public static class TwoFactorAuthentication
    {
        public static Error TotpAlreadyConfigured = Error.Conflict(
            $"{nameof(Authentication)}.{nameof(TotpAlreadyConfigured)}",
            "2FA is already configured for this user");
        
        public static Error TotpNotConfigured = Error.Conflict(
            $"{nameof(Authentication)}.{nameof(TotpNotConfigured)}",
            "2FA is not configured for this user");
        
        public static Error TotpFailure = Error.Unauthorized(
            $"{nameof(Authentication)}.{nameof(TotpFailure)}",
            "2FA verification failed");
    }
}