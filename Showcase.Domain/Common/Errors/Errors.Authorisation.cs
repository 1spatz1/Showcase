using ErrorOr;

namespace Showcase.Domain.Common.Errors;

public partial class Errors
{
    public static class Authorisation
    {
        public static Error ReCaptchaFailed = Error.Forbidden($"{nameof(Authorisation)}.{nameof(ReCaptchaFailed)}",
            "The provided reCAPTCHA token is invalid or has expired.");
        
        // Make an error that user is not allowed to acces resource
        public static Error NotAllowed = Error.Forbidden($"{nameof(Authorisation)}.{nameof(NotAllowed)}",
            "You are not allowed to access this resource");
    }
}