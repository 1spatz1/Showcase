namespace Showcase.Contracts.TwoFactorAuthentication;

public record ConfigureTotpRequest
(
    string RecaptchaToken,
    string UserId = ""
);