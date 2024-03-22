namespace Showcase.Contracts.TwoFactorAuthentication;

public record DisableTotpRequest
(
    string RecaptchaToken,
    string UserId = ""
);