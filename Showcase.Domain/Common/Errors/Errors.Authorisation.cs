using ErrorOr;

namespace Showcase.Domain.Common.Errors;

public partial class Errors
{
    public static class Authorisation
    {
        public static Error ReCaptchaFailed = Error.Forbidden($"{nameof(Authorisation)}.{nameof(ReCaptchaFailed)}",
            "The provided reCAPTCHA token is invalid or has expired.");
    }
}